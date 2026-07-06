using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();

        Task<Role?> GetRoleByIdAsync(Guid roleId);

        Task<bool> InsertRoleAsync(Role role);

        Task<bool> UpdateRoleAsync(Role role);

        Task<bool> DeleteRoleAsync(Guid roleId);
    }
}
