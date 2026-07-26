using BPM.Web.API.Helpers;
using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BPM.Web.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public string _tokenKey { get; }
        private readonly ILogger<AccountService> _logger;
        private readonly IUserLoginHistoryRepository _loginHistoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDealerService _dealerService;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger, IConfiguration configuration, IUserLoginHistoryRepository loginHistoryRepository,
            IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepository, IDealerService dealerService, IUserPasswordHistoryRepository userPasswordHistoryRepository)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _tokenKey = configuration.GetValue<string>("Jwt:Key");
            _loginHistoryRepository = loginHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepository = refreshTokenRepository;
            _dealerService = dealerService;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthenticateUserDto dto)
        {
            AuthResponse authResponse = new AuthResponse();
            UserLoginHistory loginHistory = null;
            var jwtId = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation("Login attempt for Username {Username}", dto.Username);

                var user = await _accountRepository.AuthenticateAsync(dto.Username);

                if (user != null)
                {
                    bool isPasswordValid = HashSalt.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);

                    if (isPasswordValid)
                    {
                        if (user.IsActive)
                        {
                            // Generate JWT token
                            string jwtToken = GenerateJwt(user, jwtId);

                            // Generate refresh token
                            var generatedRefreshToken = RefreshTokenHelper.GenerateRefreshToken();

                            await _refreshTokenRepository.CreateAsync(new RefreshToken
                            {
                                UserId = user.Id,
                                RefreshTokenValue = generatedRefreshToken,
                                JwtTokenId = Guid.NewGuid().ToString(),
                                CreatedOn = DateTime.UtcNow,
                                ExpiresOn = DateTime.UtcNow.AddDays(7),
                                IsRevoked = false,
                                IpAddress = GetIpAddress(),
                                UserAgent = GetUserAgent(),
                                CreatedBy = user.Id
                            });

                            // Set response
                            authResponse.JwtToken = jwtToken;
                            authResponse.RefreshToken = generatedRefreshToken;
                            authResponse.IsValidPassword = true;
                            authResponse.IsValidUser = true;
                            authResponse.Message = "Login Successful";
                            authResponse.authenticateResponseDto = new AuthenticateResponseDto()
                            {
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Phone = user.Phone,
                                RoleId = user.RoleId,
                                UserId = user.Id,
                                IsActive = user.IsActive,
                                DealerId = user.DealerId
                            };

                            if (user.DealerId != null)
                            {
                                var dealerInfo = await _dealerService.GetDealerByIdAsync(user.DealerId.Value);
                                authResponse.authenticateResponseDto.DealerInfo = dealerInfo;
                            }

                            _logger.LogInformation("User {Username} logged in successfully", dto.Username);
                        }
                        else
                        {
                            authResponse.IsValidPassword = true;
                            authResponse.IsValidUser = true;
                            authResponse.Message = "Login unsuccessful due to account is inactive";

                            _logger.LogWarning("Inactive user attempted to login. Username: {Username}", dto.Username);
                        }
                    }
                    else
                    {
                        authResponse.IsValidPassword = false;
                        authResponse.IsValidUser = true;
                        authResponse.Message = "Login unsuccessful, invalid password";

                        _logger.LogWarning("Invalid password for Username {Username}", dto.Username);
                    }
                }
                else
                {
                    authResponse.IsValidPassword = false;
                    authResponse.IsValidUser = false;
                    authResponse.Message = "Invalid user, please try with a different username";

                    _logger.LogWarning("Invalid username {Username}", dto.Username);
                }

                // Create login history entry
                loginHistory = CreateLoginHistory(dto.Username, user?.Id, authResponse);

                // Save login history outside the try-catch to ensure it's tracked
                await _loginHistoryRepository.AddAsync(loginHistory);

                return authResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while authenticating Username {Username}", dto.Username);

                // Create failure login history
                var failureHistory = new UserLoginHistory
                {
                    Username = dto.Username,
                    LoginTime = DateTime.UtcNow,
                    IsLoginSuccessful = false,
                    FailureReason = ex.Message,
                    IpAddress = GetIpAddress(),
                    UserAgent = GetUserAgent(),
                    BrowserName = GetBrowserName(),
                    OperatingSystem = GetOperatingSystem(),
                    DeviceName = Environment.MachineName,
                    SessionId = Guid.NewGuid(),
                    JwtTokenId = jwtId,
                    CreatedOn = DateTime.UtcNow
                };

                await _loginHistoryRepository.AddAsync(failureHistory);
                throw;
            }
        }



        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _accountRepository.GetUserByIdAsync(dto.UserId);

            if (user == null)
                return false;

            // Get last 5 passwords
            var passwordHistory = await _userPasswordHistoryRepository
                .GetLastFivePasswordsAsync(user.Id);

            // Verify against previous passwords
            foreach (var history in passwordHistory)
            {
                bool isPasswordUsed = HashSalt.VerifyPassword(
                    dto.NewPassword,
                    history.PasswordHash,
                    history.PasswordSalt);

                if (isPasswordUsed)
                {
                    throw new Exception("You cannot reuse any of your last 5 passwords.");
                }
            }

            // Save current password to history
            await _userPasswordHistoryRepository.AddAsync(new UserPasswordHistory
            {
                UserId = user.Id,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                CreatedOn = DateTime.UtcNow
            });

            // Generate new hash and salt
            var hashSalt = HashSalt.GenerateSaltedHash(dto.NewPassword);

            user.PasswordHash = hashSalt.Hash;
            user.PasswordSalt = hashSalt.Salt;
            user.ModifiedBy = user.Id;
            user.ModifiedOn = DateTime.UtcNow;

            await _accountRepository.UpdateUserAsync(user);

            // Keep only latest 5 passwords
            await _userPasswordHistoryRepository.DeleteOldPasswordsAsync(user.Id);

            return true;
        }

        public async Task<ForgotPasswordResponseDto> IdentifyUserAsync(ForgotPasswordDto dto)
        {
            var user = await _accountRepository.AuthenticateAsync(dto.Username);

            if (user == null)
            {
                return new ForgotPasswordResponseDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            return new ForgotPasswordResponseDto
            {
                Success = true,
                UserId = user.Id,
                Message = "User found"
            };
        }
        private string GetIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private string GetUserAgent()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Request.Headers["User-Agent"].ToString() ?? "Unknown";
        }
         
        private string GetBrowserName()
        {
            var userAgent = GetUserAgent();

            if (userAgent.Contains("Chrome", StringComparison.OrdinalIgnoreCase))
                return "Chrome";
            if (userAgent.Contains("Firefox", StringComparison.OrdinalIgnoreCase))
                return "Firefox";
            if (userAgent.Contains("Edge", StringComparison.OrdinalIgnoreCase))
                return "Edge";
            if (userAgent.Contains("Safari", StringComparison.OrdinalIgnoreCase))
                return "Safari";
            if (userAgent.Contains("Opera", StringComparison.OrdinalIgnoreCase))
                return "Opera";
            if (userAgent.Contains("Mozilla", StringComparison.OrdinalIgnoreCase))
                return "Mozilla";

            return "Unknown";
        }

        private string GetOperatingSystem()
        {
            var userAgent = GetUserAgent();

            if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
                return "Windows";
            if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
                return "Android";
            if (userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase))
                return "macOS";
            if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
                return "Linux";
            if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase))
                return "iOS";

            return "Unknown";
        }

        private UserLoginHistory CreateLoginHistory(string username, Guid? userId, AuthResponse authResponse)
        {
            var isSuccessful = !string.IsNullOrEmpty(authResponse.JwtToken);

            return new UserLoginHistory
            {
                UserId = userId,
                Username = username,
                LoginTime = DateTime.UtcNow,
                IsLoginSuccessful = isSuccessful,
                FailureReason = isSuccessful ? null : authResponse.Message,
                IpAddress = GetIpAddress(),
                UserAgent = GetUserAgent(),
                BrowserName = GetBrowserName(),
                OperatingSystem = GetOperatingSystem(),
                DeviceName = Environment.MachineName,
                SessionId = Guid.NewGuid(),
                JwtTokenId = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (token == null)
                throw new Exception("Invalid Refresh Token");

            if (token.IsRevoked)
                throw new Exception("Refresh Token Revoked");

            if (token.ExpiresOn <= DateTime.UtcNow)
                throw new Exception("Refresh Token Expired");

            var user = await _accountRepository.GetUserByIdAsync(token.UserId);

            var jwtId = Guid.NewGuid().ToString();

            var accessToken = GenerateJwt(user, jwtId);

            return new AuthResponse
            {
                JwtToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateJwt(User user, string jwtId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenKey);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
        {
            return await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        }

        public async Task<bool> UpdateAsync(RefreshToken token)
        {
            return await _refreshTokenRepository.UpdateAsync(token);
        }

        public async Task<bool> RevokeAllAsync(Guid userId)
        {
            return await _refreshTokenRepository.RevokeAllAsync(userId);
        }
    }
}