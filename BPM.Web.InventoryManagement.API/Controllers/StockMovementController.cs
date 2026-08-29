using BPM.Web.InventoryManagement.API.Models.DTOs;
using BPM.Web.InventoryManagement.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.InventoryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementController : ControllerBase
    {
        private readonly IStockMovementService _service;
        private readonly ILogger<StockMovementController> _logger;

        public StockMovementController(IStockMovementService service, ILogger<StockMovementController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-stock-movement")]
        public async Task<IActionResult> Create([FromBody] StockMovementCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating stock movement.");

                var result = await _service.CreateAsync(dto);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid stock movement request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Related record not found while creating stock movement.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating stock movement.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-stock-movements")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all stock movements.");

                var result = await _service.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all stock movements.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-stock-movement-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching stock movement by Id: {Id}", id);

                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Stock movement not found: {Id}", id);
                    return NotFound(new { message = "Stock movement not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movement: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-movements-by-inventory/{inventoryId}")]
        public async Task<IActionResult> GetByInventory(Guid inventoryId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for InventoryId: {InventoryId}", inventoryId);

                var result = await _service.GetByInventoryAsync(inventoryId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching movements for InventoryId: {InventoryId}", inventoryId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-movements-by-drug/{drugId}")]
        public async Task<IActionResult> GetByDrug(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for DrugId: {DrugId}", drugId);

                var result = await _service.GetByDrugAsync(drugId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching movements for DrugId: {DrugId}", drugId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-movements-by-warehouse/{warehouseId}")]
        public async Task<IActionResult> GetByWarehouse(Guid warehouseId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for WarehouseId: {WarehouseId}", warehouseId);

                var result = await _service.GetByWarehouseAsync(warehouseId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching movements for WarehouseId: {WarehouseId}", warehouseId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-movements-by-distributor/{distributorId}")]
        public async Task<IActionResult> GetByDistributor(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for DistributorId: {DistributorId}", distributorId);

                var result = await _service.GetByDistributorAsync(distributorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching movements for DistributorId: {DistributorId}", distributorId);
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}
