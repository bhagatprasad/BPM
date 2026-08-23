using BPM.Web.Operations.UI.Models;

namespace BPM.Web.Operations.UI.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersListAsync();
    }
}
