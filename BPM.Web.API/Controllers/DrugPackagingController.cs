using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugPackagingController : ControllerBase
    {
        private readonly IDrugPackagingService _packagingService;
        private readonly ILogger<DrugPackagingController> _logger;

        public DrugPackagingController(
            IDrugPackagingService packagingService,
            ILogger<DrugPackagingController> logger)
        {
            _packagingService = packagingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Drug Packaging");

                var data = await _packagingService.GetAllAsync();

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Packaging");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("{packagingId:guid}")]
        public async Task<IActionResult> GetById(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Fetching Drug Packaging with Id {PackagingId}", packagingId);

                var data = await _packagingService.GetByIdAsync(packagingId);

                if (data == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Packaging not found."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Packaging with Id {PackagingId}", packagingId);
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
                _logger.LogInformation("Fetching Drug Packaging for Drug Id {DrugId}", drugId);

                var data = await _packagingService.GetByDrugIdAsync(drugId);

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Packaging for Drug Id {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            try
            {
                _logger.LogInformation("Fetching Drug Packaging with Barcode {Barcode}", barcode);

                var data = await _packagingService.GetByBarcodeAsync(barcode);

                if (data == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Packaging not found with the provided barcode."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Packaging with Barcode {Barcode}", barcode);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFiltered([FromBody] DrugPackagingFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Fetching filtered Drug Packaging");

                var (items, totalCount) = await _packagingService.GetFilteredAsync(filter);

                return Ok(new
                {
                    Success = true,
                    Data = items,
                    Total = totalCount,
                    Page = filter.Page ?? 1,
                    PageSize = filter.PageSize ?? 10
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered Drug Packaging");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugPackagingDto dto)
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

                _logger.LogInformation("Creating Drug Packaging");

                var result = await _packagingService.CreateAsync(dto);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return CreatedAtAction(nameof(GetById), new { packagingId = result.Data?.PackagingId }, new
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug Packaging");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDrugPackagingDto dto)
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

                _logger.LogInformation("Updating Drug Packaging with Id {PackagingId}", dto.PackagingId);

                var result = await _packagingService.UpdateAsync(dto);

                if (!result.Success)
                {
                    return result.Message.Contains("not found")
                        ? NotFound(new { Success = false, Message = result.Message })
                        : BadRequest(new { Success = false, Message = result.Message });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug Packaging with Id {PackagingId}", dto.PackagingId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPatch("{packagingId:guid}")]
        public async Task<IActionResult> Patch(Guid packagingId, [FromBody] UpdateDrugPackagingDto dto)
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

                _logger.LogInformation("Patching Drug Packaging with Id {PackagingId}", packagingId);

                // Get existing
                var existing = await _packagingService.GetByIdAsync(packagingId);
                if (existing == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Packaging not found."
                    });
                }

                // Update only provided fields
                var updateDto = new UpdateDrugPackagingDto
                {
                    PackagingId = packagingId,
                    DrugId = dto.DrugId != Guid.Empty ? dto.DrugId : existing.DrugId,
                    PackageUomId = dto.PackageUomId != Guid.Empty ? dto.PackageUomId : existing.PackageUomId,
                    ContainsUomId = dto.ContainsUomId != Guid.Empty ? dto.ContainsUomId : existing.ContainsUomId,
                    Quantity = dto.Quantity > 0 ? dto.Quantity : existing.Quantity,
                    TotalUnits = dto.TotalUnits > 0 ? dto.TotalUnits : existing.TotalUnits,
                    UnitPrice = dto.UnitPrice > 0 ? dto.UnitPrice : existing.UnitPrice,
                    PackagePrice = dto.PackagePrice > 0 ? dto.PackagePrice : existing.PackagePrice,
                    Barcode = !string.IsNullOrEmpty(dto.Barcode) ? dto.Barcode : existing.Barcode,
                    GrossWeight = dto.GrossWeight ?? existing.GrossWeight,
                    NetWeight = dto.NetWeight ?? existing.NetWeight,
                    Length = dto.Length ?? existing.Length,
                    Width = dto.Width ?? existing.Width,
                    Height = dto.Height ?? existing.Height,
                    IsActive = dto.IsActive
                };

                var result = await _packagingService.UpdateAsync(updateDto);

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
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while patching Drug Packaging with Id {PackagingId}", packagingId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpDelete("{packagingId:guid}")]
        public async Task<IActionResult> Delete(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug Packaging with Id {PackagingId}", packagingId);

                var result = await _packagingService.DeleteAsync(packagingId);

                if (!result.Success)
                {
                    return result.Message.Contains("not found")
                        ? NotFound(new { Success = false, Message = result.Message })
                        : BadRequest(new { Success = false, Message = result.Message });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug Packaging with Id {PackagingId}", packagingId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("drug/{drugId:guid}/summary")]
        public async Task<IActionResult> GetPackagingSummary(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Getting packaging summary for Drug {DrugId}", drugId);

                var summary = await _packagingService.GetPackagingSummaryByDrugAsync(drugId);

                return Ok(new
                {
                    Success = true,
                    Data = summary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting packaging summary for Drug {DrugId}", drugId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("calculate-price")]
        public async Task<IActionResult> CalculatePrice(
            [FromQuery] Guid packageUomId,
            [FromQuery] Guid containsUomId,
            [FromQuery] int quantity,
            [FromQuery] decimal unitPrice)
        {
            try
            {
                _logger.LogInformation("Calculating package price");

                var price = await _packagingService.CalculatePackagePriceAsync(
                    packageUomId, containsUomId, quantity, unitPrice);

                return Ok(new
                {
                    Success = true,
                    TotalPrice = price
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating package price");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }
    }
}
