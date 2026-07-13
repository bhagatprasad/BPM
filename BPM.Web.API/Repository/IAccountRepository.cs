using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IAccountRepository
    {
        Task<User> AuthenticateAsync(string username);
    }
}
