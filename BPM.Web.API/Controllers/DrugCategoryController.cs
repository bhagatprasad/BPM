using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugCategoryController : ControllerBase
    {
        private readonly IDrugCategoryService _drugCategoryService;
        private readonly ILogger<DrugCategoryController> _logger;

        public DrugCategoryController(IDrugCategoryService drugCategoryService, ILogger<DrugCategoryController> logger)
        {
            _drugCategoryService = drugCategoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrugCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all drug categories.");

                var drugCategories = await _drugCategoryService.GetAllDrugCategoriesAsync();

                return Ok(drugCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all drug categories.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDrugCategory(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching drug category with Id {DrugCategoryId}", id);

                var drugCategory = await _drugCategoryService.GetDrugCategoryByIdAsync(id);

                if (drugCategory == null)
                {
                    return NotFound();
                }

                return Ok(drugCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drug category with Id {DrugCategoryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugCategoryDto drugCategoryDto)
        {
            try
            {
                _logger.LogInformation("Creating drug category.");

                var result = await _drugCategoryService.InsertDrugCategoryAsync(drugCategoryDto);

                if (!result)
                {
                    return BadRequest("Unable to create drug category.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug category.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDrugCategoryDto drugCategoryDto)
        {
            try
            {
                _logger.LogInformation("Updating drug category.");

                if (id != drugCategoryDto.Id)
                {
                    return BadRequest("Route Id and Drug Category Id do not match.");
                }

                var result = await _drugCategoryService.UpdateDrugCategoryAsync(drugCategoryDto);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug category with Id {DrugCategoryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting drug category with Id {DrugCategoryId}", id);

                var result = await _drugCategoryService.DeleteDrugCategoryAsync(id);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug category with Id {DrugCategoryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}