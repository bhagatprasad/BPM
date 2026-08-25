using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
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

        public PurchaseOrderController(IPurchaseOrderService service, ILogger<PurchaseOrderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("create-purchase-order")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Purchase Order.");
            }
        }

        [HttpGet]
        [Route("get-purchase-orders")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching purchase orders.");
            }
        }


        [HttpGet]
        [Route("get-purchase-order-by-id/{purchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderById(Guid purchaseOrderId)
        {
            try
            {
                _logger.LogInformation("Fetching purchase order with Id: {Id}", purchaseOrderId);
                var purchaseOrder = await _service.GetPurchaseOrderByIdAsync(purchaseOrderId);
                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase order not found with Id: {Id}", purchaseOrderId);
                    return NotFound("Purchase Order Not Found.");
                }
                return Ok(purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching purchase order.");
            }
        }

        [HttpGet]
        [Route("fetch-purchase-order-by-dealer/{dealerId}")]
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

        [HttpGet]
        [Route("fetch-purchase-order-by-distributor/{distributorId}")]
        public async Task<IActionResult> GetPurchaseOrdersByDistributor(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching purchase orders for Distributor Id: {DistributorId}", distributorId);
                var purchaseOrders = await _service.GetPurchaseOrdersByDistributorAsync(distributorId);
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase orders for Dealer Id: {DistributorId}", distributorId);
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

        [HttpPost]
        [Route("validate-product-availability")]
        public async Task<IActionResult> ValidateProductAvailability(ValidateProductAvailabilityDto dto)
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

        [HttpPost]
        [Route("submit-purchase-order")]
        public async Task<IActionResult> SubmitPurchaseOrder([FromBody] SubmitPurchaseOrderDto dto)
        {
            try
            {
                _logger.LogInformation("Submitting Purchase Order with Id: {Id}", dto.PurchaseOrderId);
                var currentUserId = UserId.Value;
                var result = await _service.SubmitPurchaseOrderAsync(dto, currentUserId);
                _logger.LogInformation("Purchase Order submitted successfully with Id: {Id}", dto.PurchaseOrderId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unable to submit Purchase Order with Id: {Id}", dto.PurchaseOrderId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting Purchase Order with Id: {Id}", dto.PurchaseOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while submitting the Purchase Order." });
            }
        }

        [HttpPost]
        [Route("save-purchase-order-draft")]
        public async Task<IActionResult> SavePurchaseOrderDraft([FromBody] SavePurchaseOrderDraftDto dto)
        {
            try
            {
                _logger.LogInformation("Saving Purchase Order as Draft. OrderId: {OrderId}", dto.PurchaseOrderId);
                var currentUserId = UserId.Value;
                var result = await _service.SavePurchaseOrderDraftAsync(dto, currentUserId);
                _logger.LogInformation("Purchase Order saved as Draft successfully. OrderId: {OrderId}", result.Id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unable to save Purchase Order as Draft. OrderId: {OrderId}", dto.PurchaseOrderId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving Purchase Order as Draft. OrderId: {OrderId}", dto.PurchaseOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while saving the Purchase Order as Draft." });
            }
        }

        [HttpGet]
        [Route("get-draft-purchase-orders/{dealerId}")]
        public async Task<IActionResult> GetDraftPurchaseOrders(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching draft purchase orders for Dealer Id: {DealerId}", dealerId);
                var purchaseOrders = await _service.GetDraftPurchaseOrdersAsync(dealerId);
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching draft purchase orders for Dealer Id: {DealerId}", dealerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching draft purchase orders.");
            }
        }

        [HttpDelete]
        [Route("delete-purchase-order-draft/{purchaseOrderId}")]
        public async Task<IActionResult> DeletePurchaseOrderDraft(Guid purchaseOrderId)
        {
            try
            {
                _logger.LogInformation("Deleting Draft Purchase Order with Id: {Id}", purchaseOrderId);
                var currentUserId = UserId.Value;
                var result = await _service.DeletePurchaseOrderDraftAsync(purchaseOrderId, currentUserId);

                if (!result)
                {
                    return NotFound("Draft Purchase Order Not Found.");
                }

                return Ok(new { message = "Draft Purchase Order deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unable to delete Draft Purchase Order with Id: {Id}", purchaseOrderId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Draft Purchase Order with Id: {Id}", purchaseOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the Draft Purchase Order." });
            }
        }

        [HttpPost]
        [Route("copy-purchase-order/{purchaseOrderId}")]
        public async Task<IActionResult> CopyPurchaseOrder(Guid purchaseOrderId)
        {
            try
            {
                _logger.LogInformation("Copying Purchase Order with Id: {Id}", purchaseOrderId);
                var currentUserId = UserId.Value;
                var result = await _service.CopyPurchaseOrderAsync(purchaseOrderId, currentUserId);
                _logger.LogInformation("Purchase Order copied successfully with Id: {Id}", purchaseOrderId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid purchase order copy request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Purchase Order copy validation failed.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while copying purchase order.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}