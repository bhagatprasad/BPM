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

        public SupplierController(ISupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _service.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{supplierId:guid}")]
        public async Task<IActionResult> Get(Guid supplierId)
        {
            var supplier = await _service.GetSupplierByIdAsync(supplierId);

            if (supplier == null)
                return NotFound(new { message = "Supplier not found." });

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.InsertSupplierAsync(supplierDto);

                if (!result)
                    return BadRequest(new { message = "Failed to create supplier. Supplier code may already exist." });

                return Ok(new { message = "Supplier created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the supplier.", error = ex.Message });
            }
        }

        [HttpPut("{supplierId:guid}")]
        public async Task<IActionResult> Update(Guid supplierId, [FromBody] UpdateSupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (supplierDto.Id != supplierId)
                return BadRequest(new { message = "Supplier ID mismatch." });

            try
            {
                var result = await _service.UpdateSupplierAsync(supplierDto);

                if (!result)
                    return NotFound(new { message = "Supplier not found or update failed." });

                return Ok(new { message = "Supplier updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the supplier.", error = ex.Message });
            }
        }

        [HttpDelete("{supplierId:guid}")]
        public async Task<IActionResult> Delete(Guid supplierId)
        {
            try
            {
                var result = await _service.DeleteSupplierAsync(supplierId);

                if (!result)
                    return NotFound(new { message = "Supplier not found." });

                return Ok(new { message = "Supplier deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the supplier.", error = ex.Message });
            }
        }
    }
}