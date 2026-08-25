using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Drug.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugController : BaseController
    {
        private readonly IDrugService _drugService;
        private readonly ILogger<DrugController> _logger;

        public DrugController(IDrugService drugService, ILogger<DrugController> logger)
        {
            _drugService = drugService;
            _logger = logger;
        }

        [HttpGet("get-all-drugs")]
        public async Task<IActionResult> GetAllDrugs()
        {
            try
            {
                _logger.LogInformation("Fetching all drugs");

                var drugs = await _drugService.GetAllDrugsAsync();

                return Ok(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all drugs");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("get-drug-by-id/{drugId:guid}")]
        public async Task<IActionResult> GetDrugById(Guid drugId)
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

        [HttpPost("create-drug")]
        public async Task<IActionResult> CreateDrug(DrugDto.CreateDrugDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug");

                var result = await _drugService.CreateDrugAsync(dto);

                if (!result)
                {
                    return BadRequest(false);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("update-drug")]
        public async Task<IActionResult> UpdateDrug(DrugDto.UpdateDrugDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug with Id {DrugId}", dto.DrugId);

                var result = await _drugService.UpdateDrugAsync(dto);

                if (!result)
                {
                    return NotFound("Drug not found.");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug with Id {DrugId}", dto.DrugId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("delete-drug/{drugId:guid}")]
        public async Task<IActionResult> DeleteDrug(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Deleting drug with Id {DrugId}", drugId);

                var result = await _drugService.DeleteDrugAsync(drugId);

                if (!result)
                {
                    return NotFound("Drug not found.");
                }

                return Ok(new { Message = "Drug deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug with Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
