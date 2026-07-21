using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Repository;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AccountController(IAccountService service, ILogger<AccountController> logger, IRefreshTokenRepository refreshTokenRepository)
        {
            _service = service;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
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
                _logger.LogInformation("Resetting user password.");

                var result = await _service.ResetPasswordAsync(dto);
                if (!result)
                {
                    return BadRequest(new
                    {
                        Message = "User not found."
                    });
                }

                return Ok(new
                {
                    Message = "Password reset successful."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting password.");

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while resetting the password.",
                    Details = ex.Message
                });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            try
            {
                _logger.LogInformation("Identifying user: {Username}", dto.Username);
                var result = await _service.IdentifyUserAsync(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while identifying user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while identifying the user.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var response =
                await _service.RefreshTokenAsync(request.RefreshToken);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var token = await _refreshTokenRepository
                .GetByTokenAsync(refreshToken);

            if (token == null)
                return NotFound();

            token.IsRevoked = true;
            token.RevokedOn = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(token);

            return Ok("Logout Successful");
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _refreshTokenRepository.RevokeAllAsync(userId);

            return Ok("Logged out from all devices.");
        }

    }
}
