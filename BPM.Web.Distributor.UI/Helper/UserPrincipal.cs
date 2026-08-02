using BPM.Web.Distributor.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public static class UserPrincipal
{
    public static ClaimsPrincipal GenerateUserPrincipal(AuthResponse response)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", response.AuthenticateResponseDto.UserId.ToString()),
            new Claim("FirstName", response.AuthenticateResponseDto.FirstName),
            new Claim("LastName", response.AuthenticateResponseDto.LastName),
            new Claim("Email", response.AuthenticateResponseDto.Email),
            new Claim("RoleId", response.AuthenticateResponseDto.RoleId.ToString()),
            new Claim(ClaimTypes.Role, response.AuthenticateResponseDto.RoleInfo.Name.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }
}