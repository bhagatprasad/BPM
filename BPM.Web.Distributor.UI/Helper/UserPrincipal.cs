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
            new Claim("FirstName", response.AuthenticateResponseDto.FirstName ?? string.Empty),
            new Claim("LastName", response.AuthenticateResponseDto.LastName ?? string.Empty),
            new Claim("Email", response.AuthenticateResponseDto.Email ?? string.Empty),
            new Claim("RoleId", response.AuthenticateResponseDto.RoleId.ToString()),
            new Claim(ClaimTypes.Name, $"{response.AuthenticateResponseDto.FirstName} {response.AuthenticateResponseDto.LastName}"),
            new Claim(ClaimTypes.Email, response.AuthenticateResponseDto.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, response.AuthenticateResponseDto.RoleInfo.Name)
        };
        
        if (response.AuthenticateResponseDto.DealerInfo == null)
        {
            claims.Add(new Claim("Portal", "Distributor"));
        }
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }
}