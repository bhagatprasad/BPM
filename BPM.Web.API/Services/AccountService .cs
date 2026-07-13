using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRespository _repository;

        public AccountService(IUserRespository repository)
        {
            _repository = repository;
        }

        public async Task<AuthenticateResponseDto> AuthenticateAsync(AuthenticateUserDto dto)
        {
            // Get user by Username (Email) or Phone
            var user = await _repository.GetUserByUsernameOrPhoneAsync(dto.Username, dto.Phone);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!user.IsActive)
            {
                throw new Exception("User is inactive.");
            }

            // Verify password
            bool isValidPassword = HashSalt.VerifyPassword(
                dto.Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!isValidPassword)
            {
                throw new Exception("Invalid password.");
            }

            // Return authenticated user details
            return new AuthenticateResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                IsActive = user.IsActive
            };
        }
    }
}
