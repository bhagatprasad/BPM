using BPM.Web.Identity.API.Models.Entities;

namespace BPM.Web.Identity.API.Repository
{
    public interface IUserLoginHistoryRepository
    {
        Task<bool> AddAsync(UserLoginHistory loginHistory);

        Task<List<UserLoginHistory>> GetAllAsync();

        Task<UserLoginHistory?> GetByIdAsync(Guid id);

        Task<List<UserLoginHistory>> GetByUserIdAsync(Guid userId);
    }
}
