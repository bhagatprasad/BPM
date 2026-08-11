using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetAllAsync();

        Task<PermissionDto?> GetByIdAsync(Guid permissionId);

        Task<PermissionDto> AddAsync(PermissionCreateDto dto);

        Task<PermissionDto?> UpdateAsync(Guid permissionId, PermissionUpdateDto dto);

        Task<bool> DeleteAsync(Guid permissionId);

        Task<List<PermissionFeatureDto>> GetPermissionsByRoleAsync(Guid roleId);

        Task<bool> HasPermissionAsync(Guid roleId, string featureCode, string activityCode);
    }
}
