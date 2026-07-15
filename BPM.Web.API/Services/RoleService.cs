using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all roles");

                var roles = await _roleRepository.GetAllRolesAsync();

                return roles.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all roles");
                throw;
            }
        }

        public async Task<RoleDto?> GetRoleByIdAsync(Guid roleId)
        {
            try
            {
                _logger.LogInformation("Retrieving role with Id {RoleId}", roleId);

                var role = await _roleRepository.GetRoleByIdAsync(roleId);

                if (role == null)
                {
                    _logger.LogWarning("Role not found with Id {RoleId}", roleId);
                    return null;
                }

                return role.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving role with Id {RoleId}", roleId);
                throw;
            }
        }

        public async Task<bool> InsertRoleAsync(CreateRoleDto dto)
        {
            try
            {
                _logger.LogInformation("Creating role");

                var role = dto.ToEntity();

                var result = await _roleRepository.InsertRoleAsync(role);

                if (!result)
                {
                    _logger.LogWarning("Failed to create role");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating role");
                throw;
            }
        }

        public async Task<bool> UpdateRoleAsync(UpdateRoleDto dto)
        {
            try
            {
                _logger.LogInformation("Updating role");

                var existingRole = await _roleRepository.GetRoleByIdAsync(dto.Id);

                if (existingRole == null)
                {
                    _logger.LogWarning("Role not found with Id {RoleId}", dto.Id);
                    return false;
                }

                dto.MapToEntity(existingRole);

                var result = await _roleRepository.UpdateRoleAsync(existingRole);

                if (!result)
                {
                    _logger.LogWarning("Failed to update role");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role with Id {RoleId}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            try
            {
                _logger.LogInformation("Deleting role with Id {RoleId}", roleId);

                var result = await _roleRepository.DeleteRoleAsync(roleId);

                if (!result)
                {
                    _logger.LogWarning("Role not found for deletion with Id {RoleId}", roleId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role with Id {RoleId}", roleId);
                throw;
            }
        }
    }
}