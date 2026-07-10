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
    }
}
