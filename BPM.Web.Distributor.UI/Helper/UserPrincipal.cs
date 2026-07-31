using BPM.Web.Distributor.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public static class UserPrincipal
{
    public static ClaimsPrincipal GenerateUserPrincipal(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("Email", user.Email),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }
}