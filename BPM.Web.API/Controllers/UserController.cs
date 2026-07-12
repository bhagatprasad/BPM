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
        private readonly IUserServiec _userServiec;
        public UserController(IUserServiec userServiec)
        {
            _userServiec = userServiec;
        }

        [HttpPost]
        public async Task<IActionResult> InsertUserAsync(UserCreateDto user)
        {
            var result = await _userServiec.InsertUserAsync(user);

            if (result)
            {
                return Ok(new { message = "User inserted successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to insert user." });
            }
        }
        [HttpPost]
        [Route("activate")]
        public async Task<IActionResult> ActivateUserAync(UserActivateDto userActivateDto)
        {
            var result = await _userServiec.ActivateUserAync(userActivateDto);
            if (result)
            {
                return Ok(new { message = "User activation status updated successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to update user activation status." });
            }
        }
        [HttpPost]
        [Route("deactivate")]
        public async Task<IActionResult>DeactivateUserAsync(UserDeactivateDto userDeactivateDto) 
        {
          var res=await _userServiec.DeactivateUserAync(userDeactivateDto);
            if (res)
            {
                return Ok(new {message="User deactivation status updated successfully."});
            }
            else 
            {
                return BadRequest(new { message = "Failed to update user deactivation status." });
            }
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
        { 
          var result=await _userServiec.UpdateUserAsync(userUpdateDto);
            if (result)
            {
                return Ok(new { message = "User Information updated successfully." });
            }
            else 
            {
                return BadRequest(new { message = "Failed to update user information" });
            }
        }
        [HttpPut]
        [Route("updaterole")]
        public async Task<IActionResult>UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto)
        {
            var result=await _userServiec.UpdateUserRoleAsync(userRoleUpdateDto);
            if (result)
            {
                return Ok(new { message = "User Role Updated Successfully" });
            }
            else 
            {
                return BadRequest(new { message = "Failed to update user role" });
            }
        }
        [HttpPut]
        [Route("updatedealer")]
        public async Task<IActionResult> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto)
        {
            var result = await _userServiec.UpdateUserDealerAsync(userDealerUpdateDto);

            if (result)
            {
                return Ok(new { message = "User dealer updated successfully." });
            }

            return BadRequest(new { message = "Failed to update user dealer." });
        }
        [HttpPut]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            var result = await _userServiec.ChangePasswordAsync(userChangePasswordDto);

            if (result)
            {
                return Ok(new { message = "Password changed successfully." });
            }

            return BadRequest(new { message = "Old password is incorrect or user not found." });
        }
    }
}
