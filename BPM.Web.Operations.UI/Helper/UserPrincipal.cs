using BPM.Web.Operations.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Helper
{
    public static class UserPrincipal
    {
        public static ClaimsPrincipal GenerateUserPrincipal(AuthResponse response)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", response.AuthenticateResponseDto.UserId.ToString()),
                new Claim("FirstName", response.AuthenticateResponseDto.FirstName ?? string.Empty),
                new Claim("LastName", response.AuthenticateResponseDto.LastName ?? string.Empty),
                new Claim("Email", response.AuthenticateResponseDto.Email ?? string.Empty),
                new Claim("RoleId", response.AuthenticateResponseDto.RoleId.ToString()),
                new Claim(ClaimTypes.Name, $"{response.AuthenticateResponseDto.FirstName} {response.AuthenticateResponseDto.LastName}"),
                new Claim(ClaimTypes.Email, response.AuthenticateResponseDto.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, response.AuthenticateResponseDto.RoleInfo?.Name ?? "User")
            };

            var identity = new ClaimsIdentity(claims, "BPM");
            return new ClaimsPrincipal(identity);
        }

        public static string GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            return principal?.FindFirst(claimType)?.Value ?? string.Empty;
        }

        public static string GetUserId(ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, "UserId");
        }

        public static string GetUserEmail(ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, ClaimTypes.Email);
        }

        public static string GetUserRole(ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, ClaimTypes.Role);
        }
    }
}
