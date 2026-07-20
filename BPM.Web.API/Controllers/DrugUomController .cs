using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.DrugUom;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugUomController : ControllerBase
    {
        private readonly IDrugUomService _drugUomService;
        private readonly ILogger<DrugUomController> _logger;

        public DrugUomController(
            IDrugUomService drugUomService,
            ILogger<DrugUomController> logger)
        {
            _drugUomService = drugUomService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Drug UOMs.");

                var data = await _drugUomService.GetAllAsync();

                return Ok(DrugUomMapper.ToDtoList(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug UOMs.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{uomId:guid}")]
        public async Task<IActionResult> Get(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Fetching Drug UOM with Id {UomId}", uomId);

                var data = await _drugUomService.GetByIdAsync(uomId);

                if (data == null)
                {
                    return NotFound("Drug UOM not found.");
                }

                return Ok(DrugUomMapper.ToDto(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug UOM with Id {UomId}", uomId);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("drug/{drugId:guid}")]
        public async Task<IActionResult> GetByDrug(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching Drug UOMs for Drug Id {DrugId}", drugId);

                var data = await _drugUomService.GetByDrugIdAsync(drugId);

                return Ok(DrugUomMapper.ToDtoList(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug UOMs for Drug Id {DrugId}", drugId);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugUomDto dto)
        {
            try
            {
                _logger.LogInformation("Creating Drug UOM.");

                var drugUom = DrugUomMapper.ToEntity(dto);

                var result = await _drugUomService.InsertAsync(drugUom);

                if (!result)
                {
                    return BadRequest("Failed to create Drug UOM.");
                }

                return Ok("Drug UOM Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug UOM.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDrugUomDto dto)
        {
            try
            {
                _logger.LogInformation("Updating Drug UOM.");

                var drugUom = DrugUomMapper.ToEntity(dto);

                var result = await _drugUomService.UpdateAsync(drugUom);

                if (!result)
                {
                    return NotFound(new
                    {
                        Message = "Drug UOM not found."
                    });
                }

                return Ok(new
                {
                    Message = "Drug UOM updated successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug UOM.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{uomId:guid}")]
        public async Task<IActionResult> Delete(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug UOM with Id {UomId}", uomId);

                var result = await _drugUomService.DeleteAsync(uomId);

                if (!result)
                {
                    return NotFound(new
                    {
                        Message = "Drug UOM not found."
                    });
                }

                return Ok(new
                {
                    Message = "Drug UOM deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug UOM with Id {UomId}", uomId);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}