using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetAllAsync();

        Task<PermissionDto?> GetByIdAsync(Guid permissionId);

        Task<PermissionDto?> CreateAsync(PermissionCreateDto dto);

        Task<PermissionDto?> UpdateAsync(
            Guid permissionId,
            PermissionUpdateDto dto);

        Task<bool> DeleteAsync(Guid permissionId);
    }
}