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

        public DrugCategoryController(IDrugCategoryService drugCategoryService)
        {
            _drugCategoryService = drugCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrugCategories()
        {
            var drugCategories = await _drugCategoryService.GetAllDrugCategoriesAsync();

            return Ok(drugCategories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDrugCategory(Guid id)
        {
            var drugCategory = await _drugCategoryService.GetDrugCategoryByIdAsync(id);

            if (drugCategory == null)
                return NotFound();

            return Ok(drugCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugCategoryDto drugCategoryDto)
        {
            var result = await _drugCategoryService.InsertDrugCategoryAsync(drugCategoryDto);

            if (!result)
                return BadRequest("Unable to create drug category.");

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDrugCategoryDto drugCategoryDto)
        {
            if (id != drugCategoryDto.Id)
                return BadRequest("Route Id and Drug Category Id do not match.");

            var result = await _drugCategoryService.UpdateDrugCategoryAsync(drugCategoryDto);

            if (!result)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _drugCategoryService.DeleteDrugCategoryAsync(id);

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}
