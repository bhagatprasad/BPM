using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Drug.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DrugPackagingController : ControllerBase
    {
        private readonly IDrugPackagingService _service;
        private readonly ILogger<DrugPackagingController> _logger;

        public DrugPackagingController(IDrugPackagingService service, ILogger<DrugPackagingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-all-drug-packagings")]
        public async Task<IActionResult> GetAllDrugPackagings()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drug packagings");
                return StatusCode(500, new { message = "An error occurred while retrieving drug packagings." });
            }
        }

        [HttpGet("get-drug-packaging-by-id/{packagingId}")]
        public async Task<IActionResult> GetDrugPackagingById(Guid packagingId)
        {
            try
            {
                var result = await _service.GetByIdAsync(packagingId);

                if (result == null)
                {
                    return NotFound(new { message = "Drug packaging not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packaging with Id {PackagingId}", packagingId);
                return StatusCode(500, new { message = "An error occurred while retrieving drug packaging." });
            }
        }

        [HttpGet("get-drug-packagings-by-drug-id/{drugId}")]
        public async Task<IActionResult> GetDrugPackagingsByDrugId(Guid drugId)
        {
            try
            {
                var result = await _service.GetByDrugIdAsync(drugId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings for DrugId {DrugId}", drugId);
                return StatusCode(500, new { message = "An error occurred while retrieving drug packagings." });
            }
        }

        [HttpGet("get-drug-packagings-by-package-uom-id/{packageUomId}")]
        public async Task<IActionResult> GetDrugPackagingsByPackageUomId(Guid packageUomId)
        {
            try
            {
                var result = await _service.GetByPackageUomIdAsync(packageUomId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings for PackageUomId {PackageUomId}", packageUomId);
                return StatusCode(500, new { message = "An error occurred while retrieving drug packagings." });
            }
        }

        [HttpGet("get-drug-packagings-by-contains-uom-id/{containsUomId}")]
        public async Task<IActionResult> GetDrugPackagingsByContainsUomId(Guid containsUomId)
        {
            try
            {
                var result = await _service.GetByContainsUomIdAsync(containsUomId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings for ContainsUomId {ContainsUomId}", containsUomId);
                return StatusCode(500, new { message = "An error occurred while retrieving drug packagings." });
            }
        }

        [HttpGet("get-drug-packaging-by-barcode/{barcode}")]
        public async Task<IActionResult> GetDrugPackagingByBarcode(string barcode)
        {
            try
            {
                var result = await _service.GetByBarcodeAsync(barcode);

                if (result == null)
                {
                    return NotFound(new { message = "Drug packaging not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packaging with Barcode {Barcode}", barcode);
                return StatusCode(500, new { message = "An error occurred while retrieving drug packaging." });
            }
        }

        [HttpGet("get-drug-packagings-by-price-range")]
        public async Task<IActionResult> GetDrugPackagingsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var result = await _service.GetByPriceRangeAsync(minPrice, maxPrice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings by price range");
                return StatusCode(500, new { message = "An error occurred while retrieving drug packagings." });
            }
        }

        [HttpPost("get-filtered-drug-packagings")]
        public async Task<IActionResult> GetFilteredDrugPackagings([FromBody] DrugPackagingDto.DrugPackagingFilterDto filter)
        {
            try
            {
                var result = await _service.GetFilteredAsync(filter);
                return Ok(new { items=result.Items,totalCount=result.TotalCount});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering drug packagings");
                return StatusCode(500, new { message = "An error occurred while filtering drug packagings." });
            }
        }

        [HttpPost("create-drug-packaging")]
        public async Task<IActionResult> CreateDrugPackaging([FromBody] DrugPackagingDto.CreateDrugPackagingDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug packaging");
                return StatusCode(500, new { message = "An error occurred while creating drug packaging." });
            }
        }

        [HttpPut("update-drug-packaging")]
        public async Task<IActionResult> UpdateDrugPackaging([FromBody] DrugPackagingDto.UpdateDrugPackagingDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);

                if (!result)
                {
                    return NotFound(new { message = "Drug packaging not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug packaging with Id {PackagingId}", dto.PackagingId);
                return StatusCode(500, new { message = "An error occurred while updating drug packaging." });
            }
        }

        [HttpDelete("delete-drug-packaging/{packagingId}")]
        public async Task<IActionResult> DeleteDrugPackaging(Guid packagingId)
        {
            try
            {
                var result = await _service.DeleteAsync(packagingId);

                if (!result)
                {
                    return NotFound(new { message = "Drug packaging not found." });
                }

                return Ok(new { message = "Drug packaging deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug packaging with Id {PackagingId}", packagingId);
                return StatusCode(500, new { message = "An error occurred while deleting drug packaging." });
            }
        }

        [HttpGet("get-total-packages-by-drug-id/{drugId}")]
        public async Task<IActionResult> GetTotalPackagesByDrugId(Guid drugId)
        {
            try
            {
                var result = await _service.GetTotalPackagesByDrugAsync(drugId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving total packages for DrugId {DrugId}", drugId);
                return StatusCode(500, new { message = "An error occurred while retrieving total packages." });
            }
        }
    }
}