using BPM.Web.API.Models.DTOs.Packaging;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagingMasterController : ControllerBase
    {
        private readonly IPackagingMasterService _packagingMasterService;
        private readonly ILogger<PackagingMasterController> _logger;

        public PackagingMasterController(
            IPackagingMasterService packagingMasterService,
            ILogger<PackagingMasterController> logger)
        {
            _packagingMasterService = packagingMasterService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Packaging.");

                var data = await _packagingMasterService.GetAllAsync();

                return Ok(PackagingMasterMapper.ToDtoList(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Packaging.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{packagingId:guid}")]
        public async Task<IActionResult> Get(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Fetching Packaging with Id {PackagingId}", packagingId);

                var data = await _packagingMasterService.GetByIdAsync(packagingId);

                if (data == null)
                {
                    return NotFound("Packaging not found.");
                }

                return Ok(PackagingMasterMapper.ToDto(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Packaging.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePackagingMasterDto dto)
        {
            try
            {
                _logger.LogInformation("Creating Packaging.");

                var entity = PackagingMasterMapper.ToEntity(dto);

                var result = await _packagingMasterService.InsertAsync(entity);

                if (!result)
                {
                    return BadRequest("Failed to create Packaging.");
                }

                return Ok("Packaging Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Packaging.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePackagingMasterDto dto)
        {
            try
            {
                _logger.LogInformation("Updating Packaging.");

                var entity = PackagingMasterMapper.ToEntity(dto);

                var result = await _packagingMasterService.UpdateAsync(entity);

                if (!result)
                {
                    return NotFound(new
                    {
                        Message = "Packaging not found."
                    });
                }

                return Ok(new
                {
                    Message = "Packaging updated successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Packaging.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{packagingId:guid}")]
        public async Task<IActionResult> Delete(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Deleting Packaging.");

                var result = await _packagingMasterService.DeleteAsync(packagingId);

                if (!result)
                {
                    return NotFound(new
                    {
                        Message = "Packaging not found."
                    });
                }

                return Ok(new
                {
                    Message = "Packaging deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Packaging.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}