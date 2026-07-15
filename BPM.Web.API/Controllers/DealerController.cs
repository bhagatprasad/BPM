using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerService _dealerService;
        private readonly ILogger<DealerController> _logger;

        public DealerController(IDealerService dealerService, ILogger<DealerController> logger)
        {
            _dealerService = dealerService;
            _logger = logger;
        }

        [HttpGet]
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDealer(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching dealer with Id {DealerId}", id);

                var dealer = await _dealerService.GetDealerByIdAsync(id);

                if (dealer == null)
                {
                    return NotFound();
                }

                return Ok(dealer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching dealer with Id {DealerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDealerDto dealerDto)
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDealerDto dealerDto)
        {
            try
            {
                _logger.LogInformation("Updating dealer.");

                if (id != dealerDto.Id)
                {
                    return BadRequest("Route Id and Dealer Id do not match.");
                }

                var result = await _dealerService.UpdateDealerAsync(dealerDto);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating dealer with Id {DealerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting dealer with Id {DealerId}", id);

                var result = await _dealerService.DeleteDealerAsync(id);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting dealer with Id {DealerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}