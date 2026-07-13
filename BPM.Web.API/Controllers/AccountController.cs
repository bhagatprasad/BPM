using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult>AuthenticateAsync(AuthenticateUserDto dto)
        {
            try
            {
                var response =
                    await _service.AuthenticateAsync(dto);

                return Ok(new
                {
                    Success = true,
                    Message = "Login Successful",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
         }
    }
}
