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
        private readonly ILogger<ManufacturerController> _logger;

        public ManufacturerController(IManufacturerService manufacturerService, ILogger<ManufacturerController> logger)
        {
            _manufacturerService = manufacturerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            try
            {
                _logger.LogInformation("Fetching all manufacturers.");

                var manufacturers = await _manufacturerService.GetAllManufacturersAsync();

                return Ok(manufacturers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all manufacturers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetManufacturer(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching manufacturer with Id {ManufacturerId}", id);

                var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);

                if (manufacturer == null)
                {
                    return NotFound();
                }

                return Ok(manufacturer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching manufacturer with Id {ManufacturerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateManufacturerDto manufacturerDto)
        {
            try
            {
                _logger.LogInformation("Creating manufacturer.");

                var result = await _manufacturerService.InsertManufacturerAsync(manufacturerDto);

                if (!result)
                {
                    return BadRequest("Unable to create manufacturer.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating manufacturer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateManufacturerDto manufacturerDto)
        {
            try
            {
                _logger.LogInformation("Updating manufacturer.");

                if (id != manufacturerDto.Id)
                {
                    return BadRequest("Route Id and Manufacturer Id do not match.");
                }

                var result = await _manufacturerService.UpdateManufacturerAsync(manufacturerDto);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating manufacturer with Id {ManufacturerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting manufacturer with Id {ManufacturerId}", id);

                var result = await _manufacturerService.DeleteManufacturerAsync(id);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting manufacturer with Id {ManufacturerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}