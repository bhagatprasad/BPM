using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IUserLoginHistoryRepository
    {
        Task<bool> AddAsync(UserLoginHistory loginHistory);

        Task<List<UserLoginHistory>> GetAllAsync();

        Task<UserLoginHistory?> GetByIdAsync(Guid id);

        Task<List<UserLoginHistory>> GetByUserIdAsync(Guid userId);
    }
}
