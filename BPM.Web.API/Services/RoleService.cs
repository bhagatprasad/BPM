using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
namespace BPM.Web.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(Guid roleId)
        {
            return await _roleRepository.GetRoleByIdAsync(roleId);
        }

        public async Task<bool> InsertRoleAsync(Role role)
        {
            return await _roleRepository.InsertRoleAsync(role);
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            return await _roleRepository.UpdateRoleAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            return await _roleRepository.DeleteRoleAsync(roleId);
        }
    }
}

