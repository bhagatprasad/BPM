using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BPM.Web.API.Controllers
{
    [BPMAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userServiec;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userServiec, ILogger<UserController> logger)
        {
            _userServiec = userServiec;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsersListAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _userServiec.GetAllUsersListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("get-all-users-by-dealer/{dealerId}")]
        public async Task<IActionResult> GetUsersListByDealerAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching users by dealer.");
                var users = await _userServiec.GetUsersListByDealerAsync(dealerId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users by dealer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("get-all-users-by-distributor/{distributorId}")]
        public async Task<IActionResult> GetUsersListByDistributorAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching users by distributor.");
                var users = await _userServiec.GetUserListByDistributorAsync(distributorId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users by distributor.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }


        [HttpPost]
        [Route("insert-user")]
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

                if (result != null)
                {
                    return Ok(new { data = result, message = "User information updated successfully." });
                }

                return BadRequest(new { data = result, message = "Failed to update user information." });
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

        [HttpPut]
        [Route("updatedistributor")]
        public async Task<IActionResult> UpdateUserDistributorAsync(UserDistributorUpdateDto userDistributorUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating distributor for UserId {UserId}", userDistributorUpdateDto.UserId);
                var result = await _userServiec.UpdateUserDistributorAsync(userDistributorUpdateDto);
                if (!result)
                {
                    _logger.LogWarning("Failed to update distributor for UserId {UserId}", userDistributorUpdateDto.UserId);
                    return BadRequest("Unable to update user distributor.");
                }
                return Ok(new { Message = "User distributor updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating distributor for UserId {UserId}", userDistributorUpdateDto.UserId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}