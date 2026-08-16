using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [BPMAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : BaseController
    {
        private readonly IPurchaseOrderService _service;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(
            IPurchaseOrderService service,
            ILogger<PurchaseOrderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-purchase-order")]
        public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderDto createPurchaseOrderDto)
        {
            try
            {
                _logger.LogInformation("Create Purchase Order request received.");

                var result = await _service.CreatePurchaseOrderAsync(createPurchaseOrderDto);

                _logger.LogInformation("Purchase Order created successfully.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Purchase Order.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the Purchase Order.");
            }
        }

        [HttpGet("get-purchase-orders")]
        public async Task<IActionResult> GetPurchaseOrdersAll()
        {
            try
            {
                _logger.LogInformation("Fetching all purchase orders.");

                var purchaseOrders = await _service.GetPurchaseOrdersAllAsync();

                _logger.LogInformation("Purchase orders fetched successfully.");

                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase orders.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while fetching purchase orders.");
            }
        }

        [HttpGet("get-purchase-order-by-id/{id:guid}")]
        public async Task<IActionResult> GetPurchaseOrderById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching purchase order with Id: {Id}", id);

                var purchaseOrder = await _service.GetPurchaseOrderByIdAsync(id);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase order not found with Id: {Id}", id);
                    return NotFound("Purchase Order Not Found.");
                }

                return Ok(purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase order.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while fetching purchase order.");
            }
        }

        [HttpGet("fetch-purchase-order-by-dealer/{dealerId:guid}")]
        public async Task<IActionResult> GetPurchaseOrdersByDealer(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching purchase orders for Dealer Id: {DealerId}", dealerId);

                var purchaseOrders = await _service.GetPurchaseOrdersByDealerAsync(dealerId);

                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase orders for Dealer Id: {DealerId}", dealerId);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching purchase orders.");
            }
        }

        [HttpPost]
        [Route("process-purchase-order")]
        public async Task<IActionResult> ProcessPurchaseOrderAsync(ProcessPurchaseOrderDto processPurchaseOrderDto)
        {
            try
            {
                _logger.LogInformation("Processing purchase order with Id: {Id}", processPurchaseOrderDto.PurchaseOrderId);

                var currentUserId = UserId.Value;

                var result = await _service.ProcessPurchaseOrderAsync(processPurchaseOrderDto, currentUserId);

                if (result == null)
                {
                    _logger.LogWarning("Failed to process purchase order with Id: {Id}", processPurchaseOrderDto.PurchaseOrderId);
                    return BadRequest("Failed to process purchase order.");
                }
                _logger.LogInformation("Purchase order processed successfully with Id: {Id}", processPurchaseOrderDto.PurchaseOrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing purchase order with Id: {Id}", processPurchaseOrderDto.PurchaseOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the purchase order.");
            }
        }

        [HttpPost("validate-product-availability")]
        public async Task<IActionResult> ValidateProductAvailability([FromBody] ValidateProductAvailabilityDto dto)
        {
            try
            {
                _logger.LogInformation("Validating product availability for DrugId: {DrugId}, PackagingId: {PackagingId}", dto.DrugId, dto.PackagingId);
                var result = await _service.ValidateProductAvailabilityAsync(dto.DrugId, dto.PackagingId, dto.Quantity);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid product availability request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating product availability.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}