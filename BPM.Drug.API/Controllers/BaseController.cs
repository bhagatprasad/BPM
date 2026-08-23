using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace BPM.Web.Drug.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Guid? UserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return userId;
                }

                return null;
            }
        }

        protected string Email
        {
            get
            {
                return User.FindFirst(ClaimTypes.Email)?.Value
                       ?? User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            }
        }

        protected string Role
        {
            get
            {
                return User.FindFirst(ClaimTypes.Role)?.Value
                       ?? User.FindFirst("role")?.Value;
            }
        }

        protected string Name
        {
            get
            {
                return User.FindFirst(ClaimTypes.Name)?.Value
                       ?? User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
            }
        }

        protected Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID claim not found");
            }

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new UnauthorizedAccessException("Invalid User ID format");
            }

            return userId;
        }
    }
}
