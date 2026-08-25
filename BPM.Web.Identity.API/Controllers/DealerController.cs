using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.CustomFilters;
using BPM.Web.Identity.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Identity.API.Controllers
{
    [BPMAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : BaseController
    {
        private readonly IDealerService _dealerService;
        private readonly ILogger<DealerController> _logger;

        public DealerController(IDealerService dealerService, ILogger<DealerController> logger)
        {
            _dealerService = dealerService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getalldealers")]
        public async Task<IActionResult> GetDealers()
        {
            try
            {
                _logger.LogInformation("Fetching all dealers.");

                var dealers = await _dealerService.GetAllDealersAsync();

                return Ok(dealers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all dealers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("getdealerbyid/{dealerId}")]
        public async Task<IActionResult> GetDealer(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching dealer with Id {DealerId}", dealerId);

                var dealer = await _dealerService.GetDealerByIdAsync(dealerId);

                if (dealer == null)
                {
                    return NotFound();
                }

                return Ok(dealer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching dealer with Id {DealerId}", dealerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("onboarddealer")]
        public async Task<IActionResult> Create(CreateDealerDto dealerDto)
        {
            try
            {
                _logger.LogInformation("Creating dealer.");

                var result = await _dealerService.InsertDealerAsync(dealerDto);

                if (!result)
                {
                    return BadRequest("Unable to create dealer.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating dealer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("updatedealer/{dealerId}")]
        public async Task<IActionResult> Update(Guid dealerId, DealerUpdatedDto dealerDto)
        {
            try
            {
                _logger.LogInformation("Updating dealer.");

                var result = await _dealerService.UpdateDealerAsync(dealerId, dealerDto);

                if (result != null)
                {
                    return Ok(new { data = result, message = "Dealer information updated successfully." });
                }

                return BadRequest("Route Id and Dealer Id do not match.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating dealer with Id {DealerId}", dealerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete]
        [Route("deactivatedealer/{dealerId}")]
        public async Task<IActionResult> Delete(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Deleting dealer with Id {DealerId}", dealerId);

                var result = await _dealerService.DeleteDealerAsync(dealerId);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting dealer with Id {DealerId}", dealerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
