using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServiec;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userServiec, ILogger<UserController> logger)
        {
            _userServiec = userServiec;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> InsertUserAsync(UserCreateDto user)
        {
            try
            {
                _logger.LogInformation("Creating user.");

                var result = await _userServiec.InsertUserAsync(user);

                if (result)
                {
                    return Ok(new { message = "User inserted successfully." });
                }

                return BadRequest(new { message = "Failed to insert user." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("activateuser")]
        public async Task<IActionResult> ActivateUserAync(UserActivateDto userActivateDto)
        {
            try
            {
                _logger.LogInformation("Activating user.");

                var result = await _userServiec.ActivateUserAync(userActivateDto);

                if (result)
                {
                    return Ok(new { message = "User activation status updated successfully." });
                }

                return BadRequest(new { message = "Failed to update user activation status." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while activating user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("deactivateuser")]
        public async Task<IActionResult> DeactivateUserAsync(UserDeactivateDto userDeactivateDto)
        {
            try
            {
                _logger.LogInformation("Deactivating user.");

                var result = await _userServiec.DeactivateUserAync(userDeactivateDto);

                if (result)
                {
                    return Ok(new { message = "User deactivation status updated successfully." });
                }

                return BadRequest(new { message = "Failed to update user deactivation status." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deactivating user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("updateuser/{userId}")]
        public async Task<IActionResult> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating user.");

                var result = await _userServiec.UpdateUserAsync(userId, userUpdateDto);

                if (result)
                {
                    return Ok(new { message = "User information updated successfully." });
                }

                return BadRequest(new { message = "Failed to update user information." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user with Id {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("updateuserrole/{userId}")]
        public async Task<IActionResult> UpdateUserRoleAsync(Guid userId, UserRoleUpdateDto userRoleUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating user role.");

                var result = await _userServiec.UpdateUserRoleAsync(userRoleUpdateDto);

                if (result)
                {
                    return Ok(new { message = "User role updated successfully." });
                }

                return BadRequest(new { message = "Failed to update user role." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user role for User Id {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("updateuserdealer/{userId}")]
        public async Task<IActionResult> UpdateUserDealerAsync(Guid userId, UserDealerUpdateDto userDealerUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating user dealer.");

                var result = await _userServiec.UpdateUserDealerAsync(userDealerUpdateDto);

                if (result)
                {
                    return Ok(new { message = "User dealer updated successfully." });
                }

                return BadRequest(new { message = "Failed to update user dealer." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user dealer for User Id {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("changepassword/{userId}")]
        public async Task<IActionResult> ChangePasswordAsync(Guid userId, UserChangePasswordDto userChangePasswordDto)
        {
            try
            {
                _logger.LogInformation("Changing user password.");

                var result = await _userServiec.ChangePasswordAsync(userChangePasswordDto);

                if (result)
                {
                    return Ok(new { message = "Password changed successfully." });
                }

                return BadRequest(new { message = "Failed to change password." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password for User Id {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}