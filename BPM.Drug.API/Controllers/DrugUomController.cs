using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Drug.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DrugUomController : ControllerBase
    {
        private readonly IDrugUomService _service;
        private readonly ILogger<DrugUomController> _logger;

        public DrugUomController(IDrugUomService service, ILogger<DrugUomController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-all-drug-uoms")]
        public async Task<IActionResult> GetAllDrugUomsAsync()
        {
            try
            {
                var result = await _service.GetAllDrugUomsAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drug UOMs");

                return StatusCode(500, new { message = "An error occurred while retrieving drug UOMs." });
            }
        }

        [HttpGet("get-drug-uom-by-id/{uomId}")]
        public async Task<IActionResult> GetDrugUomByIdAsync(Guid uomId)
        {
            try
            {
                var result = await _service.GetDrugUomByIdAsync(uomId);

                if (result == null)
                {
                    return NotFound(new { message = "Drug UOM not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug UOM with Id {UomId}", uomId);

                return StatusCode(500, new { message = "An error occurred while retrieving the drug UOM." });
            }
        }

        [HttpGet("get-drug-uoms-by-drug-id/{drugId}")]
        public async Task<IActionResult> GetDrugUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                var result = await _service.GetDrugUomsByDrugIdAsync(drugId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug UOMs for DrugId {DrugId}", drugId);

                return StatusCode(500, new { message = "An error occurred while retrieving drug UOMs." });
            }
        }

        [HttpGet("get-drug-uom-by-code/{drugId}/{uomCode}")]
        public async Task<IActionResult> GetDrugUomByCodeAsync(Guid drugId, string uomCode)
        {
            try
            {
                var result = await _service.GetDrugUomByCodeAsync(drugId, uomCode);

                if (result == null)
                {
                    return NotFound(new { message = "Drug UOM not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug UOM with DrugId {DrugId} and UomCode {UomCode}", drugId, uomCode);

                return StatusCode(500, new { message = "An error occurred while retrieving the drug UOM." });
            }
        }

        [HttpGet("get-base-units-by-drug-id/{drugId}")]
        public async Task<IActionResult> GetBaseUnitsByDrugIdAsync(Guid drugId)
        {
            try
            {
                var result = await _service.GetBaseUnitsByDrugIdAsync(drugId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving base units for DrugId {DrugId}", drugId);

                return StatusCode(500, new { message = "An error occurred while retrieving base units." });
            }
        }

        [HttpGet("get-purchase-uoms-by-drug-id/{drugId}")]
        public async Task<IActionResult> GetPurchaseUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                var result = await _service.GetPurchaseUomsByDrugIdAsync(drugId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase UOMs for DrugId {DrugId}", drugId);

                return StatusCode(500, new { message = "An error occurred while retrieving purchase UOMs." });
            }
        }

        [HttpGet("get-sales-uoms-by-drug-id/{drugId}")]
        public async Task<IActionResult> GetSalesUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                var result = await _service.GetSalesUomsByDrugIdAsync(drugId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sales UOMs for DrugId {DrugId}", drugId);

                return StatusCode(500, new { message = "An error occurred while retrieving sales UOMs." });
            }
        }

        [HttpPost("create-drug-uom")]
        public async Task<IActionResult> CreateDrugUomAsync([FromBody] DrugUomDto.CreateDrugUomDto dto)
        {
            try
            {
                var result = await _service.CreateDrugUomAsync(dto);

                if (!result)
                {
                    return BadRequest(new { message = "Drug UOM already exists or could not be created." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug UOM");

                return StatusCode(500, new { message = "An error occurred while creating the drug UOM." });
            }
        }

        [HttpPut("update-drug-uom")]
        public async Task<IActionResult> UpdateDrugUomAsync([FromBody] DrugUomDto.UpdateDrugUomDto dto)
        {
            try
            {
                var result = await _service.UpdateDrugUomAsync(dto);

                if (!result)
                {
                    return BadRequest(new { message = "Drug UOM could not be updated." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug UOM with Id {UomId}", dto.UomId);

                return StatusCode(500, new { message = "An error occurred while updating the drug UOM." });
            }
        }

        [HttpDelete("delete-drug-uom/{uomId}")]
        public async Task<IActionResult> DeleteDrugUomAsync(Guid uomId)
        {
            try
            {
                var result = await _service.DeleteDrugUomAsync(uomId);

                if (!result)
                {
                    return BadRequest(new { message = "Drug UOM could not be deleted. It may not exist or it has child UOMs." });
                }

                return Ok(new { message = "Drug UOM deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug UOM with Id {UomId}", uomId);

                return StatusCode(500, new { message = "An error occurred while deleting the drug UOM." });
            }
        }
    }
}
