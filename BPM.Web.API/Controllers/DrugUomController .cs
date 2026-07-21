using BPM.Web.API.Models.DTOs;
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

                return Ok(new
                {
                    Success = true,
                    Data = data.ToDtoList(),
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug UOMs.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
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
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug UOM not found."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = data.ToDto()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug UOM with Id {UomId}", uomId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("drug/{drugId:guid}")]
        public async Task<IActionResult> GetByDrug(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching Drug UOMs for Drug Id {DrugId}", drugId);

                var data = await _drugUomService.GetByDrugIdAsync(drugId);

                return Ok(new
                {
                    Success = true,
                    Data = data.ToDtoList(),
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug UOMs for Drug Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("drug/{drugId:guid}/base-units")]
        public async Task<IActionResult> GetBaseUnits(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching base units for Drug Id {DrugId}", drugId);

                var data = await _drugUomService.GetBaseUnitsByDrugIdAsync(drugId);

                return Ok(new
                {
                    Success = true,
                    Data = data.ToDtoList(),
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching base units for Drug Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("drug/{drugId:guid}/purchase-uoms")]
        public async Task<IActionResult> GetPurchaseUoms(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching purchase UOMs for Drug Id {DrugId}", drugId);

                var data = await _drugUomService.GetPurchaseUomsByDrugIdAsync(drugId);

                return Ok(new
                {
                    Success = true,
                    Data = data.ToDtoList(),
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase UOMs for Drug Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("drug/{drugId:guid}/sales-uoms")]
        public async Task<IActionResult> GetSalesUoms(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching sales UOMs for Drug Id {DrugId}", drugId);

                var data = await _drugUomService.GetSalesUomsByDrugIdAsync(drugId);

                return Ok(new
                {
                    Success = true,
                    Data = data.ToDtoList(),
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sales UOMs for Drug Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugUomDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                _logger.LogInformation("Creating Drug UOM.");

                var drugUom = dto.ToEntity();
                var result = await _drugUomService.InsertAsync(drugUom);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = drugUom.ToDto()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug UOM.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDrugUomDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                _logger.LogInformation("Updating Drug UOM.");

                var drugUom = dto.ToEntity();
                var result = await _drugUomService.UpdateAsync(drugUom);

                if (!result.Success)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = drugUom.ToDto()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug UOM.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPatch("{uomId:guid}")]
        public async Task<IActionResult> Patch(Guid uomId, [FromBody] UpdateDrugUomDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                _logger.LogInformation("Patching Drug UOM with Id {UomId}", uomId);

                var existing = await _drugUomService.GetByIdAsync(uomId);
                if (existing == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug UOM not found."
                    });
                }

                // Update only provided fields (using null checking)
                if (!string.IsNullOrEmpty(dto.UomCode))
                    existing.UomCode = dto.UomCode;
                if (!string.IsNullOrEmpty(dto.UomName))
                    existing.UomName = dto.UomName;
                if (!string.IsNullOrEmpty(dto.UomType))
                    existing.UomType = dto.UomType;
                if (dto.ParentUomId.HasValue)
                    existing.ParentUomId = dto.ParentUomId;
                if (dto.QuantityPerParent.HasValue)
                    existing.QuantityPerParent = dto.QuantityPerParent;
                if (dto.ConversionFactor > 0)
                    existing.ConversionFactor = dto.ConversionFactor;
                existing.IsBaseUnit = dto.IsBaseUnit;
                existing.IsPurchaseUom = dto.IsPurchaseUom;
                existing.IsSalesUom = dto.IsSalesUom;
                existing.IsInventoryUom = dto.IsInventoryUom;
                if (dto.DisplayOrder > 0)
                    existing.DisplayOrder = dto.DisplayOrder;
                if (!string.IsNullOrEmpty(dto.Remarks))
                    existing.Remarks = dto.Remarks;
                existing.IsActive = dto.IsActive;
                existing.ModifiedOn = DateTime.UtcNow;

                var result = await _drugUomService.UpdateAsync(existing);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = existing.ToDto()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while patching Drug UOM with Id {UomId}", uomId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpDelete("{uomId:guid}")]
        public async Task<IActionResult> Delete(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug UOM with Id {UomId}", uomId);

                var result = await _drugUomService.DeleteAsync(uomId);

                if (!result.Success)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug UOM with Id {UomId}", uomId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }
    }
}
