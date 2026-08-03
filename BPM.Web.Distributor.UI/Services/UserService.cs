using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        public UserService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public async Task<List<UserDto>> GetAllUsersListAsync()
        {
            return await _repositoryFactory.SendAsync<List<UserDto>>(HttpMethod.Get, "user/get-all-users");
        }
        public async Task<bool> ActivateUserAync(UserActivateDto userActivateDto)
        {
            return await _repositoryFactory.SendAsync<UserActivateDto, bool>(HttpMethod.Post, "user/activateuser", userActivateDto);
        }

        public async Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            var url = $"user/changepassword/{userChangePasswordDto.UserId}";
            return await _repositoryFactory.SendAsync<UserChangePasswordDto, bool>(HttpMethod.Put, url, userChangePasswordDto);
        }

        public async Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto)
        {
            return await _repositoryFactory.SendAsync<UserDeactivateDto, bool>(HttpMethod.Post, "user/deactivateuser", userDeactivateDto);
        }

        public async Task<bool> InsertUserAsync(UserCreateDto user)
        {
            return await _repositoryFactory.SendAsync<UserCreateDto, bool>(HttpMethod.Post, "user/insert-user", user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            var url = $"user/updateuser/{userId}";

            return await _repositoryFactory.SendAsync<UserUpdateDto, UserDto>(HttpMethod.Put, url, userUpdateDto);
        }
    }
}
