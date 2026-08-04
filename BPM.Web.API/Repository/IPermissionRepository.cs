using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllAsync();

        Task<Permission?> GetByIdAsync(Guid permissionId);

        Task<Permission> AddAsync(Permission permission);

        Task<Permission?> UpdateAsync(Permission permission);

        Task<bool> DeleteAsync(Guid permissionId);
    }
}
