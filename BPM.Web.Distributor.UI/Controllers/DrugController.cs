using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    [Authorize(Policy = "DistributorPortal")]
    public class DrugController : Controller
    {
        private readonly IDrugService _drugService;
        private readonly INotyfService _notyfService;
        private readonly ILogger<DrugController> _logger;
        public DrugController(IDrugService drugService, INotyfService notyfService, ILogger<DrugController> logger)
        {
            _drugService = drugService;
            _notyfService = notyfService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrugsList()
        {
            try
            {
                var drugs = await _drugService.GetAllDrugsAsync();
                return Json(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drugs.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDrugById(Guid id)
        {
            try
            {
                var drug = await _drugService.GetDrugByIdAsync(id);
                if (drug == null)
                {
                    return NotFound(new { success = false, message = "Drug not found." });
                }
                return Json(new { success = true, data = drug });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drug.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteDrug([FromBody] Guid drugId)
        {
            try
            {
                var result = await _drugService.DeleteDrugAsync(drugId);
                if (result)
                {
                    _notyfService.Success("Drug deleted successfully.");
                    return Json(new { success = true, message = "Drug deleted successfully." });
                }
                else
                {
                    _notyfService.Error("Failed to delete drug.");
                    return Json(new { success = false, message = "Failed to delete drug." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug.");
                return Json(new { success = false, message = "An error occurred while deleting drug." });
            }
        }
    }
}
