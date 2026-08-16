using BPM.Web.API.Service;
using Microsoft.AspNetCore.Mvc;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.CustomFilters;

namespace BPM.Web.API.Controllers
{
    [BPMAuthorize]
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

        [HttpGet]
        [Route("get-all-drugs")]
        public async Task<IActionResult> GetAllDrugs()
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

        [HttpGet]
        [Route("get-drug-by-id/{drugId}")]
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
        [Route("create-drug")]
        public async Task<IActionResult> Create(CreateDrugDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug.");

                var result = await _drugService.InsertDrugAsync(dto);

                if (!result)
                {
                    return BadRequest(false);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("update-drug")]
        public async Task<IActionResult> Update(UpdateDrugDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug.");

                var result = await _drugService.UpdateDrugAsync(dto);

                if (!result)
                {
                    return Ok(false);
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