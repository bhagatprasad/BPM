
using BPM.Web.Identity.API.Models.DTOs;

namespace BPM.Web.Identity.API.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersListAsync();
        Task<List<UserDto>> GetUsersListByDealerAsync(Guid dealerId);
        Task<bool> InsertUserAsync(UserCreateDto user);
        Task<bool> ActivateUserAync(UserActivateDto userActivateDto);
        Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto);
        Task<UserDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto);
        Task<bool> UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto);
        Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto);
        Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto);
    }
}
