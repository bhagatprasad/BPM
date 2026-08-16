using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.Billing;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BPMAuthorize]
    public class BillingController : BaseController
    {
        private readonly IBillingService _service;
        private readonly ILogger<BillingController> _logger;

        public BillingController(
            IBillingService service,
            ILogger<BillingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-billing/{salesOrderId:guid}")]
        public async Task<IActionResult> CreateBilling(Guid salesOrderId, CreateBillingDto createBillingDto)
        {
            try
            {
                _logger.LogInformation("Creating Billing for Sales Order: {SalesOrderId}", salesOrderId);

                if (salesOrderId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid Sales Order Id: {SalesOrderId}", salesOrderId);
                    return BadRequest("Invalid Sales Order Id.");
                }

                createBillingDto.SalesOrderId = salesOrderId;

                var currentUserId = UserId.Value;

                var result = await _service.CreateBillingAsync(createBillingDto, currentUserId);

                _logger.LogInformation("Billing created successfully for Sales Order: {SalesOrderId}", salesOrderId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Sales Order not found: {SalesOrderId}", salesOrderId);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Billing cannot be created for Sales Order: {SalesOrderId}", salesOrderId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Billing for Sales Order: {SalesOrderId}", salesOrderId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the Billing.");
            }
        }

        [HttpGet("get-billings")]
        public async Task<IActionResult> GetAllBilling()
        {
            try
            {
                _logger.LogInformation("Fetching all Billings.");

                var billings = await _service.GetAllBillingAsync();

                _logger.LogInformation("Billings fetched successfully.");

                return Ok(billings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all Billings.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while fetching Billings.");
            }
        }

        [HttpGet("get-billing-by-id/{id:guid}")]
        public async Task<IActionResult> GetBillingById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Billing with Id: {BillingId}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid Billing Id: {BillingId}", id);
                    return BadRequest("Invalid Billing Id.");
                }

                var billing = await _service.GetBillingByIdAsync(id);

                if (billing == null)
                {
                    _logger.LogWarning("Billing not found with Id: {BillingId}", id);
                    return NotFound("Billing not found.");
                }

                _logger.LogInformation("Billing fetched successfully with Id: {BillingId}", id);

                return Ok(billing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Billing with Id: {BillingId}", id);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while fetching the Billing.");
            }
        }

        [HttpGet("get-billing-by-sales-order/{sales-order-id:guid}")]
        public async Task<IActionResult> GetBillingBySalesOrder(Guid salesOrderId)
        {
            try
            {
                _logger.LogInformation("Fetching Billing for Sales Order: {SalesOrderId}", salesOrderId);

                if (salesOrderId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid Sales Order Id: {SalesOrderId}", salesOrderId);
                    return BadRequest("Invalid Sales Order Id.");
                }

                var billing = await _service.GetBillingBySalesOrderIdAsync(salesOrderId);

                if (billing == null)
                {
                    _logger.LogWarning("Billing not found for Sales Order: {SalesOrderId}", salesOrderId);
                    return NotFound("Billing not found for this Sales Order.");
                }

                _logger.LogInformation("Billing fetched successfully for Sales Order: {SalesOrderId}", salesOrderId);

                return Ok(billing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Billing for Sales Order: {SalesOrderId}", salesOrderId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while fetching Billing.");
            }
        }
    }
}