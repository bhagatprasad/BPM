using BPM.Web.Identity.API.Models.DTOs;

namespace BPM.Web.Identity.API.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();

        Task<RoleDto?> GetRoleByIdAsync(Guid roleId);

        Task<bool> InsertRoleAsync(CreateRoleDto role);

        Task<bool> UpdateRoleAsync(UpdateRoleDto role);

        Task<bool> DeleteRoleAsync(Guid roleId);
    }
}
