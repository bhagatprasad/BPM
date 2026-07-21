using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IAccountRepository
    {
        Task<User> AuthenticateAsync(string username);
        Task UpdateUserAsync(User user);

        Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);

        Task<bool> UpdateAsync(RefreshToken refreshToken);
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
