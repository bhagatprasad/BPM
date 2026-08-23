using BPM.Web.Operations.UI.Models;
using System.Net.Http;

namespace BPM.Web.Operations.UI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        public UserService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public async Task<List<UserDto>> GetAllUsersListAsync()
        {
            return await _repositoryFactory.SendAsync<List<UserDto>>(HttpMethod.Get, "user/get-all-users");
        }
    }
}
