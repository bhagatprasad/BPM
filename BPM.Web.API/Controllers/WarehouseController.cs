using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<WarehouseController> _logger;

        public WarehouseController(IWarehouseService warehouseService, ILogger<WarehouseController> logger)
        {
            _warehouseService = warehouseService;
            _logger = logger;
        }

        [HttpPost("create-warehouse")]
        public async Task<IActionResult> Create(WarehouseCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating warehouse with Code: {WarehouseCode}", dto.WarehouseCode);

                var result = await _warehouseService.CreateAsync(dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Warehouse creation failed for Code: {WarehouseCode}", dto.WarehouseCode);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating warehouse.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-warehouses")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all warehouses.");

                var result = await _warehouseService.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all warehouses.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-warehouse-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting warehouse with Id: {WarehouseId}", id);

                var result = await _warehouseService.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Warehouse not found with Id: {WarehouseId}", id);
                    return NotFound(new { message = "Warehouse not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting warehouse with Id: {WarehouseId}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-warehouse-by-distributor/{distributorId}")]
        public async Task<IActionResult> GetByDistributorId(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Getting warehouses for DistributorId: {DistributorId}", distributorId);

                var result = await _warehouseService.GetByDistributorIdAsync(distributorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting warehouses for DistributorId: {DistributorId}", distributorId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpPut("update-warehouse/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WarehouseUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(new { message = "Warehouse ID mismatch." });
                }

                _logger.LogInformation("Updating warehouse with Id: {WarehouseId}", id);

                var result = await _warehouseService.UpdateAsync(dto);

                if (result == null)
                {
                    _logger.LogWarning("Warehouse not found with Id: {WarehouseId}", id);
                    return NotFound(new { message = "Warehouse not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating warehouse with Id: {WarehouseId}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpDelete("delete-warehouse/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting warehouse with Id: {WarehouseId}", id);

                var result = await _warehouseService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Warehouse not found with Id: {WarehouseId}", id);
                    return NotFound(new { message = "Warehouse not found." });
                }

                return Ok(new { message = "Warehouse deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting warehouse with Id: {WarehouseId}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}