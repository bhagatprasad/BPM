using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IUserRespository
    {
        Task<bool> InsertUserAsync(User user);
        Task<bool> ActivateUserAync(User user);
        Task<bool> DeactivateUserAync(User user);
        Task<bool>UpdateUserInfoAsync(User user);
        Task<bool>UpdateUserRoleAsync(User user);
        Task<bool> UpdateUserDealerAsync(User user);
        Task<bool> ChangePasswordAsync(User user);
    }
}
