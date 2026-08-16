using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BPMAuthorize]
    public class SupplierDiscountController : BaseController
    {
        private readonly ISupplierDiscountService _service;
        private readonly ILogger<SupplierDiscountController> _logger;

        public SupplierDiscountController(
            ISupplierDiscountService service,
            ILogger<SupplierDiscountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-supplier-discount")]
        public async Task<IActionResult> Create([FromBody] SupplierDiscountCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid supplier discount request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier discount.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-supplier-discounts")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier discounts.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-supplier-discount-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = "Supplier discount not found." });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid supplier discount Id.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier discount.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-supplier-discounts-by-supplier/{supplierId}")]
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
                _logger.LogError(ex, "Error occurred while fetching supplier discounts.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}
