using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
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

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _tokenKey = configuration.GetValue<string>("Jwt:Key");
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthenticateUserDto dto)
        {
            try
            {
                _logger.LogInformation("Login attempt for Username {Username}", dto.Username);

                AuthResponse authResponse = new AuthResponse();

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
                                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                  new Claim(ClaimTypes.Name, dto.Username),
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

                return authResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while authenticating Username {Username}", dto.Username);
                throw;
            }
        }
    }
}