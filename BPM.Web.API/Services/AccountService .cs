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
            bool isLoginSuccessful = false;
            string? failureReason = null;
            var jwtId = Guid.NewGuid().ToString();
            Guid sessionId = Guid.NewGuid();
            AuthResponse authResponse = new AuthResponse();
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
                            var tokenhandler = new JwtSecurityTokenHandler();

                            var tokenkey = Encoding.ASCII.GetBytes(_tokenKey);

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                  new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                  new Claim(ClaimTypes.Name, dto.Username ?? string.Empty),
                                  new Claim(ClaimTypes.Role, user.RoleId.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddHours(1),
                                SigningCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(tokenkey),
                                    SecurityAlgorithms.HmacSha256Signature)
                            };

                            var token = tokenhandler.CreateToken(tokenDescriptor);

                            var writtoken = tokenhandler.WriteToken(token);


                            authResponse.JwtToken = writtoken;
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
                var context = _httpContextAccessor.HttpContext;

                var ipAddress = context?.Connection.RemoteIpAddress?.ToString();

                var userAgent = context?.Request.Headers["User-Agent"].ToString();

                string browser = "Unknown";

                if (userAgent.Contains("Chrome"))
                    browser = "Chrome";
                else if (userAgent.Contains("Firefox"))
                    browser = "Firefox";
                else if (userAgent.Contains("Edge"))
                    browser = "Edge";
                else if (userAgent.Contains("Mozilla"))
                    browser = "Mozilla";
                    
                var operatingSystem = "Unknown";
                if (userAgent.Contains("Windows"))
                    operatingSystem = "Windows";
                else if (userAgent.Contains("Android"))
                    operatingSystem = "Android";
                else if (userAgent.Contains("Mac"))
                    operatingSystem = "macOS";
                else if (userAgent.Contains("Linux"))
                    operatingSystem = "Linux";

                await _loginHistoryRepository.AddAsync(new UserLoginHistory
                {
                    UserId = user?.Id,
                    Username = dto.Username,
                    LoginTime = DateTime.UtcNow,
                    IsLoginSuccessful = true,
                    FailureReason = failureReason,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    BrowserName = browser,
                    DeviceName = Environment.MachineName,
                    OperatingSystem = operatingSystem,
                    SessionId = sessionId,
                    JwtTokenId = jwtId
                });
                return authResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while authenticating Username {Username}", dto.Username);
                await _loginHistoryRepository.AddAsync(new UserLoginHistory
                {
                    Username = dto.Username,
                    LoginTime = DateTime.UtcNow,
                    IsLoginSuccessful = false,
                    FailureReason = "Authentication Exception",
                    SessionId = sessionId,
                    CreatedOn = DateTime.UtcNow
                });

                throw;
            }
        }
    }
}