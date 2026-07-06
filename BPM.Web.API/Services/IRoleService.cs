using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();

        Task<Role?> GetRoleByIdAsync(Guid roleId);

        Task<bool> InsertRoleAsync(Role role);

        Task<bool> UpdateRoleAsync(Role role);

        Task<bool> DeleteRoleAsync(Guid roleId);
    }
}
