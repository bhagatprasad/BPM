using BPM.Web.API.Models.Entities;
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
            return Ok(await _service.GetAllSuppliersAsync());
        }

        [HttpGet("{supplierId:guid}")]
        public async Task<IActionResult> Get(Guid supplierId)
        {
            var supplier = await _service.GetSupplierByIdAsync(supplierId);

            if (supplier == null)
                return NotFound("Supplier not found.");

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Supplier supplier)
        {
            var result = await _service.InsertSupplierAsync(supplier);

            if (!result)
                return BadRequest("Failed to create supplier.");

            return Ok("Supplier Created Successfully");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Supplier supplier)
        {
            var result = await _service.UpdateSupplierAsync(supplier);

            if (!result)
                return NotFound("Supplier not found.");

            return Ok("Supplier Updated Successfully");
        }

        [HttpDelete("{supplierId:guid}")]
        public async Task<IActionResult> Delete(Guid supplierId)
        {
            var result = await _service.DeleteSupplierAsync(supplierId);

            if (!result)
                return NotFound("Supplier not found.");

            return Ok("Supplier Deleted Successfully");
        }
    }
}
