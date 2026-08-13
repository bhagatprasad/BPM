using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();

        Task<RoleDto> GetRoleByIdAsync(Guid id);

        Task<bool> CreateRoleAsync(CreateRoleDto dto);

        Task<bool> UpdateRoleAsync(Guid id, UpdateRoleDto dto);

        Task<bool> DeleteRoleAsync(Guid id);
    }
}