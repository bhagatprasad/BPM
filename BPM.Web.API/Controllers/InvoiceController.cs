using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.Invoice;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BPMAuthorize]
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _service;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService service, ILogger<InvoiceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-invoice/{billingId:guid}")]
        public async Task<IActionResult> CreateInvoice(Guid billingId, CreateInvoiceDto createInvoiceDto)
        {
            try
            {
                _logger.LogInformation("Creating Invoice for BillingId: {BillingId}", billingId);

                if (billingId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid BillingId: {BillingId}", billingId);
                    return BadRequest("Invalid Billing Id.");
                }

                createInvoiceDto.BillingId = billingId;

                var invoice = await _service.CreateInvoiceAsync(createInvoiceDto, UserId.Value);

                _logger.LogInformation("Invoice created successfully for BillingId: {BillingId}", billingId);

                return Ok(invoice);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Billing not found: {BillingId}", billingId);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invoice cannot be created for BillingId: {BillingId}", billingId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Invoice for BillingId: {BillingId}", billingId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Invoice.");
            }
        }

        [HttpGet("get-invoices")]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                _logger.LogInformation("Fetching all invoices.");

                var invoices = await _service.GetAllInvoiceAsync();

                _logger.LogInformation("Invoices fetched successfully.");

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching invoices.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching invoices.");
            }
        }

        [HttpGet("get-invoice-by-id/{id:guid}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Invoice with Id: {InvoiceId}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid InvoiceId: {InvoiceId}", id);
                    return BadRequest("Invalid Invoice Id.");
                }

                var invoice = await _service.GetInvoiceByIdAsync(id);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice not found with Id: {InvoiceId}", id);
                    return NotFound("Invoice not found.");
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Invoice with Id: {InvoiceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Invoice.");
            }
        }

        [HttpGet("get-invoice-by-billing/{billingId:guid}")]
        public async Task<IActionResult> GetInvoiceByBilling(Guid billingId)
        {
            try
            {
                _logger.LogInformation("Fetching Invoice for BillingId: {BillingId}", billingId);

                if (billingId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid BillingId: {BillingId}", billingId);
                    return BadRequest("Invalid Billing Id.");
                }

                var invoice = await _service.GetInvoiceByBillingIdAsync(billingId);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice not found for BillingId: {BillingId}", billingId);
                    return NotFound("Invoice not found for this Billing.");
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Invoice for BillingId: {BillingId}", billingId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Invoice.");
            }
        }
    }
}
