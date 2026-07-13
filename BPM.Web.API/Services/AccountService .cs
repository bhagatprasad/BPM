using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthenticateUserDto dto)
        {
            AuthResponse authResponse = new AuthResponse();

            var user = await _accountRepository.AuthenticateAsync(dto.Username);

            if (user != null)
            {
                bool IsPasswordValid = HashSalt.VerifyPassword(dto.Password,  user.PasswordHash, user.PasswordSalt);

                if (IsPasswordValid)
                {
                    // retun auth response dto

                    if (user.IsActive)
                    {
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
                    }
                    else
                    {
                        authResponse.IsValidPassword = true;
                        authResponse.IsValidUser = true;
                        authResponse.Message = "Login unsuccessful due account is inactive";
                    }
                }
                else
                {
                    authResponse.IsValidPassword = false;
                    authResponse.IsValidUser = true;
                    authResponse.Message = "Login UnSuccessful,Invalid password";
                }
            }
            else
            {
                authResponse.IsValidPassword = false;
                authResponse.IsValidUser = false;
                authResponse.Message = "Invalid user, please try with deffernt username";
            }

            return authResponse;
        }
    }
}
