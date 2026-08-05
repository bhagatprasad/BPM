using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [BPMAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : BaseController
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _activityService.GetAllAsync());
        }

        [HttpGet("{activityId}")]
        public async Task<IActionResult> GetById(Guid activityId)
        {
            var result = await _activityService.GetByIdAsync(activityId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActivityCreateDto dto)
        {
            var result = await _activityService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("{activityId}")]
        public async Task<IActionResult> Update(Guid activityId, ActivityUpdateDto dto)
        {
            var result = await _activityService.UpdateAsync(activityId, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(Guid activityId)
        {
            var result = await _activityService.DeleteAsync(activityId);

            if (!result)
                return NotFound();

            return Ok("Activity deleted successfully.");
        }
    }
}