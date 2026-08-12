using BPM.Web.API.Models.DTOs;
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

        Task<bool> HasPermissionAsync(Guid roleId, string featureCode, string activityCode);

        Task<List<PermissionFeatureDto>> GetPermissionsByRoleAsync(Guid roleId);


    }
}
