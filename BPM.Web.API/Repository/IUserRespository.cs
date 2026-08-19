using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IUserRespository
    {
        Task<User> InsertUserAsync(User user);
        Task<bool> ActivateUserAync(User user);
        Task<bool> DeactivateUserAync(User user);
        Task<bool> UpdateUserInfoAsync(User user);
        Task<bool> UpdateUserRoleAsync(User user);
        Task<bool> UpdateUserDealerAsync(User user);
        Task<bool> ChangePasswordAsync(User user);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUserListByDealerAsync(Guid dealerId);
        Task<List<User>> GetUserListByDistributorAsync(Guid distributorId);
        Task<User> GetUserByUsernameOrPhoneAsync(string username, string phone);
        Task<bool> UpdateUserDistributorAsync(User user);
    }
}
