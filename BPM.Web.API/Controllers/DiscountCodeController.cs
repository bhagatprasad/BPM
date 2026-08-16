using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BPMAuthorize]
    public class DiscountCodeController : BaseController
    {
        private readonly IDiscountCodeService _service;
        private readonly ILogger<DiscountCodeController> _logger;

        public DiscountCodeController(
            IDiscountCodeService service,
            ILogger<DiscountCodeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-discount-code")]
        public async Task<IActionResult> Create([FromBody] DiscountCodeCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid discount code request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating discount code.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-discount-codes")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching discount codes.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-discount-code-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = "Discount code not found." });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid discount code Id.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching discount code.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-discount-code-by-code/{discountCode}")]
        public async Task<IActionResult> GetByCode(string discountCode)
        {
            try
            {
                var result = await _service.GetByCodeAsync(discountCode);

                if (result == null)
                {
                    return NotFound(new { message = "Discount code not found." });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid discount code.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching discount code.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-discount-codes-by-supplier/{supplierId}")]
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
                _logger.LogError(ex, "Error occurred while fetching discount codes.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-active-discount-codes")]
        public async Task<IActionResult> GetActiveCodes()
        {
            try
            {
                var result = await _service.GetActiveCodesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active discount codes.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}
