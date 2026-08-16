using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BPMAuthorize]
    public class PromotionalOfferController : BaseController
    {
        private readonly IPromotionalOfferService _service;
        private readonly ILogger<PromotionalOfferController> _logger;

        public PromotionalOfferController(
            IPromotionalOfferService service,
            ILogger<PromotionalOfferController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-promotional-offer")]
        public async Task<IActionResult> Create([FromBody] PromotionalOfferCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid promotional offer request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating promotional offer.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-promotional-offers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offers.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-promotional-offer-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = "Promotional offer not found." });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid promotional offer Id.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offer.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-promotional-offers-by-supplier/{supplierId}")]
        public async Task<IActionResult> GetBySupplier(Guid supplierId)
        {
            try
            {
                var result = await _service.GetBySupplierAsync(supplierId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid SupplierId.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offers.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-promotional-offers-by-drug/{drugId}")]
        public async Task<IActionResult> GetByDrug(Guid drugId)
        {
            try
            {
                var result = await _service.GetByDrugAsync(drugId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid DrugId.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offers.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-active-promotional-offers")]
        public async Task<IActionResult> GetActiveOffers()
        {
            try
            {
                var result = await _service.GetActiveOffersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active promotional offers.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}
