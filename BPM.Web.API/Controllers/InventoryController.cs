using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BPMAuthorize]
    public class InventoryController : BaseController
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpPost("create-inventory")]
        public async Task<IActionResult> Create([FromBody] InventoryCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating inventory for DrugId: {DrugId}, WarehouseId: {WarehouseId}", dto.DrugId, dto.WarehouseId);

                var result = await _inventoryService.CreateAsync(dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Inventory creation failed.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating inventory.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-inventories")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all inventories.");

                var result = await _inventoryService.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all inventories.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-inventory-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting inventory with Id: {InventoryId}", id);

                var result = await _inventoryService.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Inventory not found with Id: {InventoryId}", id);
                    return NotFound(new { message = "Inventory not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventory with Id: {InventoryId}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-inventory-by-distributor/{distributorId}")]
        public async Task<IActionResult> GetByDistributorId(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Getting inventories for DistributorId: {DistributorId}", distributorId);

                var result = await _inventoryService.GetByDistributorIdAsync(distributorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventories for DistributorId: {DistributorId}", distributorId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-inventory-by-drug/{drugId}")]
        public async Task<IActionResult> GetByDrugId(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Getting inventories for DrugId: {DrugId}", drugId);

                var result = await _inventoryService.GetByDrugIdAsync(drugId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventories for DrugId: {DrugId}", drugId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-inventory-by-warehouse/{warehouseId}")]
        public async Task<IActionResult> GetByWarehouseId(Guid warehouseId)
        {
            try
            {
                _logger.LogInformation("Getting inventories for WarehouseId: {WarehouseId}", warehouseId);

                var result = await _inventoryService.GetByWarehouseIdAsync(warehouseId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventories for WarehouseId: {WarehouseId}", warehouseId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromQuery] InventoryAvailabilityDto dto)
        {
            try
            {
                _logger.LogInformation("Checking inventory availability for DrugId: {DrugId}, WarehouseId: {WarehouseId}", dto.DrugId, dto.WarehouseId);

                var result = await _inventoryService.CheckAvailabilityAsync(dto);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Inventory not found while checking availability.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking inventory availability.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpPut("update-inventory/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] InventoryUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(new { message = "Inventory ID mismatch." });
                }

                _logger.LogInformation("Updating inventory with Id: {InventoryId}", id);

                var result = await _inventoryService.UpdateAsync(dto);

                if (result == null)
                {
                    _logger.LogWarning("Inventory not found with Id: {InventoryId}", id);
                    return NotFound(new { message = "Inventory not found." });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Inventory update failed for Id: {InventoryId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory with Id: {InventoryId}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpDelete("delete-inventory/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting inventory with Id: {InventoryId}", id);

                var result = await _inventoryService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Inventory not found with Id: {InventoryId}", id);
                    return NotFound(new { message = "Inventory not found." });
                }

                return Ok(new { message = "Inventory deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory with Id: {InventoryId}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}