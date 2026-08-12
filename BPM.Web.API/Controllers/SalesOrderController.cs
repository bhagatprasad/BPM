using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.SalesOrder;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BPMAuthorize]
    public class SalesOrderController : BaseController
    {
        private readonly ISalesOrderService _service;
        private readonly ILogger<SalesOrderController> _logger;

        public SalesOrderController(ISalesOrderService service, ILogger<SalesOrderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("GetSalesOrders")]
        public async Task<IActionResult> GetAllSalesOrder()
        {
            try
            {
                _logger.LogInformation("Fetching all Sales Orders");
                var salesorders = await _service.GetAllSalesOrderAsync();
                _logger.LogInformation("Sales Orders fetched Successfully");
                return Ok(salesorders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occurred while fetching Sales Orders");


                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching SalesOrders");
            }
        }

        [HttpGet("GetSalesOrderByDealer/{dealerId}")]
        public async Task<IActionResult> GetSalesOrderByDealerId(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching Sales Orders for DealerId: {DealerId}", dealerId);

                if (dealerId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid DealerId provided: {DealerId}", dealerId);

                    return BadRequest("Invalid DealerId.");
                }

                var salesorders = await _service.GetSalesOrderByDealerAsync(dealerId);

                _logger.LogInformation("Sales Orders fetched successfully for DealerId: {DealerId}", dealerId);

                return Ok(salesorders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Sales Orders for DealerId: {DealerId}", dealerId);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching Sales Orders.");
            }
        }

        [HttpPost("CreateSalesOrderFromPurchaseOrder/{purchaseOrderId}")]
        public async Task<IActionResult> CreateSalesOrderFromPurchaseOrder(Guid purchaseOrderId)
        {
            try
            {
                _logger.LogInformation("Creating Sales Order from Purchase Order: {PurchaseOrderId}", purchaseOrderId);

                if (purchaseOrderId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid PurchaseOrderId: {PurchaseOrderId}", purchaseOrderId);
                    return BadRequest("Invalid Purchase Order Id.");
                }

                var salesOrder = await _service.CreateSalesOrderFromPurchaseOrderAsync(purchaseOrderId, UserId.Value);

                return CreatedAtAction(nameof(GetAllSalesOrder), new { id = salesOrder.Id }, salesOrder);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Purchase Order not found: {PurchaseOrderId}", purchaseOrderId);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Purchase Order cannot be converted: {PurchaseOrderId}", purchaseOrderId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Sales Order from Purchase Order: {PurchaseOrderId}", purchaseOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Sales Order.");
            }
        }

        [HttpGet("GetSalesOrderById/{id}")]
        public async Task<IActionResult> GetSalesOrderById(Guid id)
        {
            try
            {
                var salesOrder =await _service.GetSalesOrderByIdAsync(id);

                if (salesOrder == null)
                {
                    return NotFound("Sales Order not found.");
                }

                return Ok(salesOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Sales Order with Id: {SalesOrderId}",id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Sales Order.");
            }
        }

        [HttpPost("process-sales-order")]
        public async Task<IActionResult> ProcessSalesOrder(ProcessSalesOrderDto processSalesOrderDto)
        {
            try
            {
                _logger.LogInformation("Processing Sales Order with Id: {SalesOrderId}", processSalesOrderDto.SalesOrderId);

                if (processSalesOrderDto.SalesOrderId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid Sales Order Id: {SalesOrderId}", processSalesOrderDto.SalesOrderId);
                    return BadRequest("Invalid Sales Order Id.");
                }

                var currentUserId = UserId.Value;

                var result = await _service.ProcessSalesOrderAsync(processSalesOrderDto, currentUserId);

                _logger.LogInformation("Sales Order processed successfully with Id: {SalesOrderId}", processSalesOrderDto.SalesOrderId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Sales Order not found: {SalesOrderId}", processSalesOrderDto.SalesOrderId);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Sales Order cannot be processed: {SalesOrderId}", processSalesOrderDto.SalesOrderId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing Sales Order: {SalesOrderId}", processSalesOrderDto.SalesOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the Sales Order.");
            }
        }
    }
}
