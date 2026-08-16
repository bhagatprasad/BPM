using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BPMAuthorize]
    public class VolumeDiscountTierController : BaseController
    {
        private readonly IVolumeDiscountTierService _service;
        private readonly ILogger<VolumeDiscountTierController> _logger;

        public VolumeDiscountTierController(
            IVolumeDiscountTierService service,
            ILogger<VolumeDiscountTierController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create-volume-discount-tier")]
        public async Task<IActionResult> Create([FromBody] VolumeDiscountTierCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid volume discount tier request.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating volume discount tier.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-all-volume-discount-tiers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching volume discount tiers.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-volume-discount-tier-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = "Volume discount tier not found." });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid volume discount tier Id.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching volume discount tier.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [HttpGet("get-volume-discount-tiers-by-supplier/{supplierId}")]
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
                _logger.LogError(ex, "Error occurred while fetching volume discount tiers.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}
