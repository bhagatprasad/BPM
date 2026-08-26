using BPM.Web.Billing.API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Billing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        [HttpPost]
        [Route("create-billing")]
        public async Task<IActionResult> CreateBillingAsync(CreateBillingDto billing)
        {
            var response = new BillingResponseDto();

            return Ok(response);
        }
    }
}
