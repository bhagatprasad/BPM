using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Drug.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugCategoryController : BaseController
    {
        private readonly IDrugCategoryService _drugCategoryService;
        private readonly ILogger<DrugCategoryController> _logger;

        public DrugCategoryController(IDrugCategoryService drugCategoryService, ILogger<DrugCategoryController> logger)
        {
            _drugCategoryService = drugCategoryService;
            _logger = logger;
        }

        [HttpGet("get-all-drug-categories")]
        public async Task<IActionResult> GetAllDrugCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all drug categories");
                var categories = await _drugCategoryService.GetAllDrugCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all drug categories");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("get-drug-category-by-id/{id:guid}")]
        public async Task<IActionResult> GetDrugCategoryById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching drug category with Id {CategoryId}", id);
                var category = await _drugCategoryService.GetDrugCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound("Drug category not found.");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drug category with Id {CategoryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost("create-drug-category")]
        public async Task<IActionResult> CreateDrugCategory(DrugCategoryDto.CreateDrugCategoryDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug category");
                var result = await _drugCategoryService.CreateDrugCategoryAsync(dto);
                if (!result)
                {
                    return BadRequest(false);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug category");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("update-drug-category")]
        public async Task<IActionResult> UpdateDrugCategory(DrugCategoryDto.UpdateDrugCategoryDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug category with Id {CategoryId}", dto.Id);
                var result = await _drugCategoryService.UpdateDrugCategoryAsync(dto);
                if (!result)
                {
                    return NotFound("Drug category not found.");
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug category with Id {CategoryId}", dto.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("delete-drug-category/{id:guid}")]
        public async Task<IActionResult> DeleteDrugCategory(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting drug category with Id {CategoryId}", id);
                var result = await _drugCategoryService.DeleteDrugCategoryAsync(id);
                if (!result)
                {
                    return NotFound("Drug category not found.");
                }
                return Ok(new { Message = "Drug category deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug category with Id {CategoryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}