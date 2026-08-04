using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionDto>> GetAllAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();

            return permissions.Select(x => new PermissionDto
            {
                PermissionId = x.PermissionId,
                RoleId = x.RoleId,
                FeatureId = x.FeatureId,
                ActivityId = x.ActivityId,
                IsEnabled = x.IsEnabled,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn
            }).ToList();
        }

        public async Task<PermissionDto?> GetByIdAsync(Guid permissionId)
        {
            var permission = await _permissionRepository.GetByIdAsync(permissionId);

            if (permission == null)
                return null;

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                RoleId = permission.RoleId,
                FeatureId = permission.FeatureId,
                ActivityId = permission.ActivityId,
                IsEnabled = permission.IsEnabled,
                CreatedBy = permission.CreatedBy,
                CreatedOn = permission.CreatedOn,
                ModifiedBy = permission.ModifiedBy,
                ModifiedOn = permission.ModifiedOn
            };
        }

        public async Task<PermissionDto> AddAsync(PermissionCreateDto dto)
        {
            var permission = new Permission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = dto.RoleId,
                FeatureId = dto.FeatureId,
                ActivityId = dto.ActivityId,
                IsEnabled = dto.IsEnabled,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            var result = await _permissionRepository.AddAsync(permission);

            return await GetByIdAsync(result.PermissionId);
        }

        public async Task<PermissionDto?> UpdateAsync(Guid permissionId, PermissionUpdateDto dto)
        {
            var permission = new Permission
            {
                PermissionId = permissionId,
                RoleId = dto.RoleId,
                FeatureId = dto.FeatureId,
                ActivityId = dto.ActivityId,
                IsEnabled = dto.IsEnabled,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _permissionRepository.UpdateAsync(permission);

            if (result == null)
                return null;

            return await GetByIdAsync(permissionId);
        }

        public async Task<bool> DeleteAsync(Guid permissionId)
        {
            return await _permissionRepository.DeleteAsync(permissionId);
        }
    }
}
