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
            var user = userActivateDto.ToEntity();

            return await _userRespository.ActivateUserAync(user);
        }

        public async Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto)
        {
            var user = userDeactivateDto.ToEntity();

            return await _userRespository.DeactivateUserAync(user);
        }

        public async Task<bool> InsertUserAsync(UserCreateDto user)
        {
            var newUser = user.ToEntity();

            return await _userRespository.InsertUserAsync(newUser);
        }

        public async Task<bool> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            userUpdateDto.UserId = userId;
            var user = userUpdateDto.ToEntity();
            return await _userRespository.UpdateUserInfoAsync(user);
        }

        public async Task<bool> UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto)
        {
            var user = userRoleUpdateDto.ToEntity();
            return await _userRespository.UpdateUserRoleAsync(user);

        }

        public async Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto)
        {
            var user = userDealerUpdateDto.ToEntity();
            return await _userRespository.UpdateUserDealerAsync(user);

        }

        public async Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            var changedUser = userChangePasswordDto.ToEntity();

            return await _userRespository.ChangePasswordAsync(changedUser);
        }
    }
}
