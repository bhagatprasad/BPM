using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IFeatureService _featureService;
        private readonly IActivityService _activityService;

        public PermissionController(
            IPermissionService permissionService,
            IRoleService roleService,
            IFeatureService featureService,
            IActivityService activityService)
        {
            _permissionService = permissionService;
            _roleService = roleService;
            _featureService = featureService;
            _activityService = activityService;
        }

        // =========================================================
        // GET: /Permission/GetPermissionsByRole/{roleId}
        // =========================================================

        [HttpGet]
        [Route("Permission/GetPermissionsByRole/{roleId:guid}")]
        public async Task<IActionResult> GetPermissionsByRole(Guid roleId)
        {
            var permissions =
                await _permissionService.GetAllAsync();

            var roles =
                await _roleService.GetAllRolesAsync();

            var features =
                await _featureService.GetAllFeaturesAsync();

            var activities =
                await _activityService.GetAllActivitiesAsync();

            var role =
                roles.FirstOrDefault(x => x.Id == roleId);

            if (role == null)
            {
                return NotFound();
            }

            var rolePermissions =
                permissions
                    .Where(x => x.RoleId == roleId)
                    .ToList();

            var result =
                rolePermissions
                    .Select(permission =>
                    {
                        var feature =
                            features.FirstOrDefault(
                                x => x.FeatureId == permission.FeatureId);

                        var activity =
                            activities.FirstOrDefault(
                                x => x.ActivityId == permission.ActivityId);

                        return new PermissionDto
                        {
                            PermissionId =
                                permission.PermissionId,

                            RoleId =
                                permission.RoleId,

                            RoleName =
                                role.Name,

                            FeatureId =
                                permission.FeatureId,

                            FeatureName =
                                feature?.FeatureName
                                ?? "Unknown Feature",

                            ActivityId =
                                permission.ActivityId,

                            ActivityName =
                                activity?.ActivityName
                                ?? "Unknown Activity",

                            IsEnabled =
                                permission.IsEnabled,

                            CreatedBy =
                                permission.CreatedBy,

                            CreatedOn =
                                permission.CreatedOn,

                            ModifiedBy =
                                permission.ModifiedBy,

                            ModifiedOn =
                                permission.ModifiedOn
                        };
                    })
                    .ToList();

            return View("Index", result);
        }


        // =========================================================
        // PUT: /Permission/Update/{permissionId}
        // =========================================================

        [HttpPut]
        [Route("Permission/Update/{permissionId:guid}")]
        public async Task<IActionResult> Update(
            Guid permissionId,
            [FromBody] PermissionUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Permission data is required.");
            }

            try
            {
                var result =
                    await _permissionService.UpdateAsync(
                        permissionId,
                        dto);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}