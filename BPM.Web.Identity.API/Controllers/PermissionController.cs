using BPM.Web.Identity.API.CustomFilters;
using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Route("get-all-permissions")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _permissionService.GetAllAsync());
        }

        [HttpGet]
        [Route("get-permissions-by-id/{permissionId}")]
        public async Task<IActionResult> GetById(Guid permissionId)
        {
            var result = await _permissionService.GetByIdAsync(permissionId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Route("create-permission")]
        public async Task<IActionResult> Create([FromBody] PermissionCreateDto dto)
        {
            var result = await _permissionService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        [Route("update-permission/{permissionId}")]
        public async Task<IActionResult> Update(Guid permissionId, [FromBody] PermissionUpdateDto dto)
        {
            var result = await _permissionService.UpdateAsync(permissionId, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete-permission/{permissionId}")]
        public async Task<IActionResult> Delete(Guid permissionId)
        {
            var result = await _permissionService.DeleteAsync(permissionId);

            if (!result)
                return NotFound();

            return Ok("Permission deleted successfully.");
        }

        [HttpGet]
        [Route("get-permissions-by-role/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRole(Guid roleId)
        {
            var result = await _permissionService.GetPermissionsByRoleAsync(roleId);

            return Ok(result);
        }

        [HttpGet]
        [Route("has-permission")]
        public async Task<IActionResult> HasPermission([FromQuery] Guid roleId,[FromQuery] string featureCode,[FromQuery] string activityCode)
        {
            var result = await _permissionService.HasPermissionAsync(
                roleId,
                featureCode,
                activityCode);

            return Ok(result);
        }
    }
}
