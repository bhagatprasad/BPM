using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger, IConfiguration configuration, IUserLoginHistoryRepository loginHistoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _tokenKey = configuration.GetValue<string>("Jwt:Key");
            _loginHistoryRepository = loginHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthenticateUserDto dto)
        {
            AuthResponse authResponse = new AuthResponse();
            UserLoginHistory loginHistory = null;

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
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var tokenKey = Encoding.ASCII.GetBytes(_tokenKey);

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, dto.Username ?? string.Empty),
                            new Claim(ClaimTypes.Role, user.RoleId.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddHours(1),
                                SigningCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(tokenKey),
                                    SecurityAlgorithms.HmacSha256Signature)
                            };

                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            var jwtToken = tokenHandler.WriteToken(token);

                            // Generate refresh token
                            var refreshToken = GenerateRefreshToken();

                            // Save refresh token to database
                            //await _refreshTokenRepository.AddAsync(new RefreshToken
                            //{
                            //    Token = refreshToken,
                            //    UserId = user.Id,
                            //    Expires = DateTime.UtcNow.AddDays(7),
                            //    Created = DateTime.UtcNow,
                            //    CreatedByIp = GetIpAddress(),
                            //    IsRevoked = false,
                            //    IsUsed = false,
                            //    JwtTokenId = Guid.NewGuid().ToString()
                            //});

                            // Set response
                            authResponse.JwtToken = jwtToken;
                            authResponse.RefreshToken = refreshToken;
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
                                IsActive = user.IsActive
                            };

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
                    JwtTokenId = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.UtcNow
                };

                await _loginHistoryRepository.AddAsync(failureHistory);
                throw;
            }
        }



        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _accountRepository.AuthenticateAsync(dto.Username);

            if (user == null)
                return false;

            var hashSalt = HashSalt.GenerateSaltedHash(dto.NewPassword);

            user.PasswordHash = hashSalt.Hash;
            user.PasswordSalt = hashSalt.Salt;

            user.ModifiedBy = user.Id;
            user.ModifiedOn = DateTime.UtcNow;

            await _accountRepository.UpdateUserAsync(user);

            return true;
        }
        // Helper methods
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
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
    }
}