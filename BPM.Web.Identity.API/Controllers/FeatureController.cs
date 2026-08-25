using BPM.Web.Identity.API.CustomFilters;
using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Identity.API.Controllers
{
    [BPMAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController : BaseController
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _featureService.GetAllAsync());
        }

        [HttpGet("{featureId:guid}")]
        public async Task<IActionResult> GetById(Guid featureId)
        {
            var result = await _featureService.GetByIdAsync(featureId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeatureCreateDto dto)
        {
            var result = await _featureService.AddAsync(dto);

            return Ok(result);
        }

        [HttpPut("{featureId:guid}")]
        public async Task<IActionResult> Update(Guid featureId, [FromBody] FeatureUpdateDto dto)
        {
            var result = await _featureService.UpdateAsync(featureId, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{featureId:guid}")]
        public async Task<IActionResult> Delete(Guid featureId)
        {
            var result = await _featureService.DeleteAsync(featureId);

            if (!result)
                return NotFound();

            return Ok("Feature deleted successfully.");
        }
    }
}
