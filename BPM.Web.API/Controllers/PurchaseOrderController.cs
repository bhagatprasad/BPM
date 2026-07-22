using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
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

        [HttpPost("CreatePurchaseOrder")]
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

        [HttpGet("FetchPurchaseOrders")]
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

        [HttpGet("FetchPurchaseOrderById/{id:guid}")]
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

        [HttpGet("FetchPurchaseOrderByDealer/{dealerId:guid}")]
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

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while fetching purchase orders.");
            }
        }
    }
}