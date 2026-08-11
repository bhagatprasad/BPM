using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [BPMAuthorize]
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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _permissionService.GetAllAsync());
        }

        [HttpGet("{permissionId:guid}")]
        public async Task<IActionResult> GetById(Guid permissionId)
        {
            var result = await _permissionService.GetByIdAsync(permissionId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionCreateDto dto)
        {
            var result = await _permissionService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("{permissionId:guid}")]
        public async Task<IActionResult> Update(Guid permissionId, [FromBody] PermissionUpdateDto dto)
        {
            var result = await _permissionService.UpdateAsync(permissionId, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{permissionId:guid}")]
        public async Task<IActionResult> Delete(Guid permissionId)
        {
            var result = await _permissionService.DeleteAsync(permissionId);

            if (!result)
                return NotFound();

            return Ok("Permission deleted successfully.");
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRole(Guid roleId)
        {
            var result = await _permissionService.GetPermissionsByRoleAsync(roleId);

            return Ok(result);
        }

        [HttpGet("HasPermission")]
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
