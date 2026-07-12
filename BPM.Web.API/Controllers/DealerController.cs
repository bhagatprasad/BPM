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
            _logger.LogInformation("Fetching all dealers.");
            var dealers = await _dealerService.GetAllDealersAsync();
            _logger.LogInformation($"Fetched {dealers.Count} dealers.");
            return Ok(dealers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDealer(Guid id)
        {
            _dealerService.GetDealerByIdAsync(id);
            var dealer = await _dealerService.GetDealerByIdAsync(id);

            if (dealer == null)
                return NotFound();

            return Ok(dealer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDealerDto dealerDto)
        {
            var result = await _dealerService.InsertDealerAsync(dealerDto);

            if (!result)
                return BadRequest("Unable to create dealer.");

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDealerDto dealerDto)
        {
            if (id != dealerDto.Id)
                return BadRequest("Route Id and Dealer Id do not match.");

            var result = await _dealerService.UpdateDealerAsync(dealerDto);

            if (!result)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _dealerService.DeleteDealerAsync(id);

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}