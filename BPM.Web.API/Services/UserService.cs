using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRespository _userRespository;
        public UserService(IUserRespository userRespository)
        {
            _userRespository = userRespository;
        }

        public async Task<bool> ActivateUserAync(UserActivateDto userActivateDto)
        {
            return await _userRespository.ActivateUserAync(userActivateDto);
        }

        public async Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto)
        {
            return await _userRespository.DeactivateUserAync(userDeactivateDto);
        }

        public async Task<bool> InsertUserAsync(UserCreateDto user)
        {
            var newUser = user.ToEntity();

            return await _userRespository.InsertUserAsync(newUser);
        }

        public async Task<bool> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            return await _userRespository.UpdateUserInfoAsync(userUpdateDto);
        }

        public async Task<bool> UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto)
        {
            return await _userRespository.UpdateUserRoleAsync(userRoleUpdateDto);
        }
        public async Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto)
        {
            return await _userRespository.UpdateUserDealerAsync(userDealerUpdateDto);
        }
        public async Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            var changedUser = userChangePasswordDto.ToEntity();

            return await _userRespository.ChangePasswordAsync(changedUser);
        }
    }
}
