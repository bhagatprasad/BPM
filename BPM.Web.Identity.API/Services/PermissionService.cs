using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.Models.Entities;
using BPM.Web.Identity.API.Repository;
using BPM.Web.Identity.API.Services;

namespace BPM.Web.Identity.API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<IPackagingMasterService> _logger;
        public PermissionService(IPermissionRepository permissionRepository, ILogger<PackagingMasterService> logger)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
        }

        public async Task<List<PermissionDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all permissions.");

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all permissions.");
                throw;
            }
        }

        public async Task<PermissionDto?> GetByIdAsync(Guid permissionId)
        {
            try
            {
                _logger.LogInformation("Getting permission with Id: {PermissionId}", permissionId);

                var permission = await _permissionRepository.GetByIdAsync(permissionId);

                if (permission == null)
                {
                    _logger.LogWarning("Permission not found. PermissionId: {PermissionId}", permissionId);
                    return null;
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting permission.");
                throw;
            }
        }

        public async Task<PermissionDto> AddAsync(PermissionCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating new permission.");

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

                _logger.LogInformation("Permission created successfully. PermissionId: {PermissionId}", result.PermissionId);

                return await GetByIdAsync(result.PermissionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating permission.");
                throw;
            }
        }

        public async Task<PermissionDto?> UpdateAsync(Guid permissionId, PermissionUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating permission. PermissionId: {PermissionId}", permissionId);

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
                {
                    _logger.LogWarning("Permission not found for update. PermissionId: {PermissionId}", permissionId);
                    return null;
                }

                _logger.LogInformation("Permission updated successfully. PermissionId: {PermissionId}", permissionId);

                return await GetByIdAsync(permissionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating permission.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid permissionId)
        {
            try
            {
                _logger.LogInformation("Deleting permission. PermissionId: {PermissionId}", permissionId);

                var result = await _permissionRepository.DeleteAsync(permissionId);

                if (!result)
                {
                    _logger.LogWarning("Permission not found for deletion. PermissionId: {PermissionId}", permissionId);
                    return false;
                }

                _logger.LogInformation("Permission deleted successfully. PermissionId: {PermissionId}", permissionId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting permission.");
                throw;
            }
        }

        public async Task<List<PermissionFeatureDto>> GetPermissionsByRoleAsync(Guid roleId)
        {
            try
            {
                _logger.LogInformation("Getting permissions for RoleId: {RoleId}", roleId);

                var result = await _permissionRepository.GetPermissionsByRoleAsync(roleId);

                _logger.LogInformation("Retrieved {Count} feature permission(s) for RoleId: {RoleId}", result.Count, roleId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting permissions by role.");
                throw;
            }
        }

        public async Task<bool> HasPermissionAsync(Guid roleId,string featureCode,string activityCode)
        {
            try
            {
                _logger.LogInformation(
                    "Checking permission for RoleId: {RoleId}, Feature: {FeatureCode}, Activity: {ActivityCode}",
                    roleId,
                    featureCode,
                    activityCode);

                var hasPermission = await _permissionRepository.HasPermissionAsync(
                    roleId,
                    featureCode,
                    activityCode);

                if (!hasPermission)
                {
                    _logger.LogWarning(
                        "Permission denied. RoleId: {RoleId}, Feature: {FeatureCode}, Activity: {ActivityCode}",
                        roleId,
                        featureCode,
                        activityCode);
                }
                else
                {
                    _logger.LogInformation(
                        "Permission granted. RoleId: {RoleId}, Feature: {FeatureCode}, Activity: {ActivityCode}",
                        roleId,
                        featureCode,
                        activityCode);
                }

                return hasPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while checking permission. RoleId: {RoleId}, Feature: {FeatureCode}, Activity: {ActivityCode}",
                    roleId,
                    featureCode,
                    activityCode);

                throw;
            }
        }
    }
}
