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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService service, ILogger<AccountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateUserDto dto)
        {
            try
            {
                _logger.LogInformation($"Authenticating user: {dto.Username}");

                var response = await _service.AuthenticateAsync(dto);


                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while processing the authentication request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                var result = await _service.ResetPasswordAsync(dto);

                if (!result)
                    return BadRequest("User not found.");

                return Ok(new
                {
                    Message = "Password updated successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());

                return StatusCode(500, new
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }

    }
}
