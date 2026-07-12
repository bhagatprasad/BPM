using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IUserRespository
    {
        Task<bool> InsertUserAsync(User user);
        Task<bool> ActivateUserAync(UserActivateDto userActivateDto);
        Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto);
        Task<bool>UpdateUserInfoAsync(UserUpdateDto userUpdateDto);
        Task<bool>UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto);
        Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto);
        Task<bool> ChangePasswordAsync(User user);
    }
}
