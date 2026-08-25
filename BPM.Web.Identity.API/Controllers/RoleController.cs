using BPM.Web.Identity.API.CustomFilters;
using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-all-roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                _logger.LogInformation("Fetching all roles.");

                var roles = await _roleService.GetAllRolesAsync();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all roles.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("get-role-by-id/{roleId}")]
        public async Task<IActionResult> GetRole(Guid roleId)
        {
            try
            {
                _logger.LogInformation("Fetching role with Id {RoleId}", roleId);

                var role = await _roleService.GetRoleByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound();
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching role with Id {RoleId}", roleId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto roleDto)
        {
            try
            {
                _logger.LogInformation("Creating role.");

                var result = await _roleService.InsertRoleAsync(roleDto);

                if (!result)
                {
                    return BadRequest("Unable to create role.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating role.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("update-role/{roleId}")]
        public async Task<IActionResult> Update(Guid roleId, [FromBody] UpdateRoleDto roleDto)
        {
            try
            {
                _logger.LogInformation("Updating role.");

                if (roleId != roleDto.Id)
                {
                    return BadRequest("Route Id and Role Id do not match.");
                }

                var result = await _roleService.UpdateRoleAsync(roleDto);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role with Id {RoleId}", roleId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete]
        [Route("delete-role/{roleId}")]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            try
            {
                _logger.LogInformation("Deleting role with Id {RoleId}", roleId);

                var result = await _roleService.DeleteRoleAsync(roleId);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role with Id {RoleId}", roleId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}