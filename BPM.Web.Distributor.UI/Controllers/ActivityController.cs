using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
   // [Authorize(Policy = "DistributorPortal")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivityController> _logger;
        private readonly INotyfService _notyf;

        public ActivityController(
            IActivityService activityService,
            ILogger<ActivityController> logger,
            INotyfService notyf)
        {
            _activityService = activityService;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivities()
        {
            try
            {
                _logger.LogInformation("Fetching all activities.");

                var activities = await _activityService.GetAllActivitiesAsync();

                return Json(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activities.");

                _notyf.Error("Unable to load activities.");

                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching activity {Id}", id);

                var activity = await _activityService.GetActivityByIdAsync(id);

                if (activity == null)
                    return NotFound();

                return Json(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity.");

                _notyf.Error("Unable to load activity.");

                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating activity.");

                var activity = await _activityService.CreateActivityAsync(dto);

                if (activity == null)
                    return BadRequest();

                _notyf.Success("Activity created successfully.");

                return Ok(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating activity.");

                _notyf.Error("Unable to create activity.");

                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ActivityUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating activity {Id}", id);

                var activity = await _activityService.UpdateActivityAsync(id, dto);

                if (activity == null)
                    return NotFound();

                _notyf.Success("Activity updated successfully.");

                return Ok(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating activity.");

                _notyf.Error("Unable to update activity.");

                return StatusCode(500);
            }
        }

    }
}