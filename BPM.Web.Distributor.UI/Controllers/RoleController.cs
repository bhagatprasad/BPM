using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly INotyfService _notyf;

        public RoleController(
            IRoleService roleService,
            ILogger<RoleController> logger,
            INotyfService notyf)
        {
            _roleService = roleService;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();

                return Json(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching roles.");

                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);

                if (role == null)
                    return NotFound();

                return Json(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role {RoleId}.", id);

                return StatusCode(500, "Unable to load role.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            try
            {
                var result = await _roleService.CreateRoleAsync(dto);

                if (!result)
                    return BadRequest("Unable to create role.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role.");

                return StatusCode(500, "Unable to create role.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("Route Id and Role Id do not match.");

                var result = await _roleService.UpdateRoleAsync(id, dto);

                if (!result)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role {RoleId}.", id);

                return StatusCode(500, "Unable to update role.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);

                if (!result)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {RoleId}.", id);

                return StatusCode(500, "Unable to delete role.");
            }
        }
    }
}