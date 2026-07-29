using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BPM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid UserId
        {
            get
            {
                return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
        }

        protected string Email
        {
            get
            {
                return User.FindFirst(ClaimTypes.Email)?.Value;
            }
        }

        protected string Role
        {
            get
            {
                return User.FindFirst(ClaimTypes.Role)?.Value;
            }
        }

    }
}
