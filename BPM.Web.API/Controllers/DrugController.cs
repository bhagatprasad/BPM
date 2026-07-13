using BPM.Web.API.Models.Entities;
using BPM.Web.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugController : ControllerBase
    {
        private readonly IDrugService _drugService;
        private readonly ILogger<DrugController> _logger;

        public DrugController(IDrugService drugService, ILogger<DrugController> logger)
        {
            _drugService = drugService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all drugs.");

                var drugs = await _drugService.GetAllDrugsAsync();

                return Ok(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all drugs.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{drugId:guid}")]
        public async Task<IActionResult> Get(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching drug with Id {DrugId}", drugId);

                var drug = await _drugService.GetDrugByIdAsync(drugId);

                if (drug == null)
                {
                    return NotFound("Drug not found.");
                }

                return Ok(drug);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drug with Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Drug drug)
        {
            try
            {
                _logger.LogInformation("Creating drug.");

                var result = await _drugService.InsertDrugAsync(drug);

                if (!result)
                {
                    return BadRequest("Failed to create drug.");
                }

                return Ok("Drug Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Drug drug)
        {
            try
            {
                _logger.LogInformation("Updating drug.");

                if (drug == null)
                {
                    return BadRequest("Invalid request.");
                }

                var result = await _drugService.UpdateDrugAsync(drug);

                if (!result)
                {
                    return NotFound(new
                    {
                        Message = "Drug not found."
                    });
                }

                return Ok(new
                {
                    Message = "Drug updated successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug with Id {DrugId}", drug.DrugId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{drugId:guid}")]
        public async Task<IActionResult> Delete(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Deleting drug with Id {DrugId}", drugId);

                var result = await _drugService.DeleteDrugAsync(drugId);

                if (!result)
                {
                    return NotFound(new
                    {
                        Message = "Drug not found."
                    });
                }

                return Ok(new
                {
                    Message = "Drug deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug with Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}