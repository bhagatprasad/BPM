using BPM.Web.Identity.API.Models.Entities;

namespace BPM.Web.Identity.API.Repository
{
    public interface IUserPasswordHistoryRepository
    {
        Task<List<UserPasswordHistory>> GetLastFivePasswordsAsync(Guid userId);

        Task AddAsync(UserPasswordHistory history);

        Task DeleteOldPasswordsAsync(Guid userId);
    }
}
