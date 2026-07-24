using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IUserPasswordHistoryRepository
    {
        Task<List<UserPasswordHistory>> GetLastFivePasswordsAsync(Guid userId);

        Task AddAsync(UserPasswordHistory history);

        Task DeleteOldPasswordsAsync(Guid userId);
    }
}
