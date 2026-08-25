using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.Models.Entities;

namespace BPM.Web.Identity.API.Repository
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllAsync();

        Task<Permission?> GetByIdAsync(Guid permissionId);

        Task<Permission> AddAsync(Permission permission);

        Task<Permission?> UpdateAsync(Permission permission);

        Task<bool> DeleteAsync(Guid permissionId);

        Task<bool> HasPermissionAsync(Guid roleId, string featureCode, string activityCode);

        Task<List<PermissionFeatureDto>> GetPermissionsByRoleAsync(Guid roleId);


    }
}
