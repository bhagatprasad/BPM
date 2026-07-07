using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
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

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();

            return roles.ToDto();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(Guid roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            return role?.ToDto();
        }

        public async Task<bool> InsertRoleAsync(CreateRoleDto dto)
        {
            var role = dto.ToEntity();

            return await _roleRepository.InsertRoleAsync(role);
        }

        public async Task<bool> UpdateRoleAsync(UpdateRoleDto dto)
        {
            var existingRole = await _roleRepository.GetRoleByIdAsync(dto.Id);

            if (existingRole == null)
                return false;

            dto.MapToEntity(existingRole);

            return await _roleRepository.UpdateRoleAsync(existingRole);
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            return await _roleRepository.DeleteRoleAsync(roleId);
        }
    }
}

