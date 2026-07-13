using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _service;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService service, ILogger<SupplierController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all suppliers.");

                var suppliers = await _service.GetAllSuppliersAsync();

                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all suppliers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{supplierId:guid}")]
        public async Task<IActionResult> Get(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Fetching supplier with Id {SupplierId}", supplierId);

                var supplier = await _service.GetSupplierByIdAsync(supplierId);

                if (supplier == null)
                {
                    return NotFound(new { message = "Supplier not found." });
                }

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier with Id {SupplierId}", supplierId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto supplierDto)
        {
            try
            {
                _logger.LogInformation("Creating supplier.");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _service.InsertSupplierAsync(supplierDto);

                if (!result)
                {
                    return BadRequest(new { message = "Failed to create supplier. Supplier code may already exist." });
                }

                return Ok(new { message = "Supplier created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{supplierId:guid}")]
        public async Task<IActionResult> Update(Guid supplierId, [FromBody] UpdateSupplierDto supplierDto)
        {
            try
            {
                _logger.LogInformation("Updating supplier.");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (supplierDto.Id != supplierId)
                {
                    return BadRequest(new { message = "Supplier ID mismatch." });
                }

                var result = await _service.UpdateSupplierAsync(supplierDto);

                if (!result)
                {
                    return NotFound(new { message = "Supplier not found or update failed." });
                }

                return Ok(new { message = "Supplier updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating supplier with Id {SupplierId}", supplierId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{supplierId:guid}")]
        public async Task<IActionResult> Delete(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Deleting supplier with Id {SupplierId}", supplierId);

                var result = await _service.DeleteSupplierAsync(supplierId);

                if (!result)
                {
                    return NotFound(new { message = "Supplier not found." });
                }

                return Ok(new { message = "Supplier deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting supplier with Id {SupplierId}", supplierId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}