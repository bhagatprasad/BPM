using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    //[Authorize(Policy = "DistributorPortal")]
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;
        private readonly ILogger<FeatureController> _logger;
        private readonly INotyfService _notyf;

        public FeatureController(
            IFeatureService featureService,
            ILogger<FeatureController> logger,
            INotyfService notyf)
        {
            _featureService = featureService;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFeatures()
        {
            try
            {
                _logger.LogInformation("Fetching all features.");

                var features = await _featureService.GetAllFeaturesAsync();

                return Json(features);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching features.");

                _notyf.Error("Unable to load features.");

                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid featureId)
        {
            try
            {
                var feature = await _featureService.GetFeatureByIdAsync(featureId);

                if (feature == null)
                    return NotFound();

                return Json(feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching feature.");

                _notyf.Error("Unable to load feature.");

                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeatureCreateDto dto)
        {
            try
            {;
                _logger.LogInformation("Creating feature.");
                var result = await _featureService.CreateFeatureAsync(dto);

                _notyf.Success("Feature created successfully.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating feature.");

                _notyf.Error("Unable to create feature.");

                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid featureId, [FromBody] FeatureUpdateDto dto)
        {
            try
            {
                var result = await _featureService.UpdateFeatureAsync(featureId, dto);

                if (result == null)
                    return NotFound();

                _notyf.Success("Feature updated successfully.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feature.");

                _notyf.Error("Unable to update feature.");

                return StatusCode(500, "Internal Server Error");
            }
        }

        
    }
}