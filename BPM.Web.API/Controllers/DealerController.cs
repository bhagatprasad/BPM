using BPM.Web.API.Models.Entities;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerService _dealerService;

        public DealerController(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDealers()
        {
            var dealers = await _dealerService.GetAllDealersAsync();

            return Ok(dealers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealer(Guid id)
        {
            var dealer = await _dealerService.GetDealerByIdAsync(id);

            if (dealer == null)
                return NotFound();

            return Ok(dealer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dealer dealer)
        {
            var result = await _dealerService.InsertDealerAsync(dealer);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Dealer dealer)
        {
            var result = await _dealerService.UpdateDealerAsync(dealer);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _dealerService.DeleteDealerAsync(id);

            return Ok(result);
        }
    }
}
