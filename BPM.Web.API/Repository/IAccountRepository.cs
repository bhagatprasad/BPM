using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IAccountRepository
    {
        Task<User> AuthenticateAsync(string username);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
