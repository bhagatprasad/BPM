using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();

            return Ok(manufacturers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetManufacturer(Guid id)
        {
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);

            if (manufacturer == null)
                return NotFound();

            return Ok(manufacturer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateManufacturerDto manufacturerDto)
        {
            var result = await _manufacturerService.InsertManufacturerAsync(manufacturerDto);

            if (!result)
                return BadRequest("Unable to create manufacturer.");

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateManufacturerDto manufacturerDto)
        {
            if (id != manufacturerDto.Id)
                return BadRequest("Route Id and Manufacturer Id do not match.");

            var result = await _manufacturerService.UpdateManufacturerAsync(manufacturerDto);

            if (!result)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _manufacturerService.DeleteManufacturerAsync(id);

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}