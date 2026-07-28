using Microsoft.AspNetCore.Authentication.Cookies;
using BPM.Web.Distributor.UI.Models;
using System.Security.Claims;

namespace BPM.Web.Distributor.UI.Helpers
{
    public static class UserPrincipal
    {
        //public static ClaimsPrincipal GenerateUserPrincipal(ApplicationUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim("Id", user.Id.ToString()),
        //        new Claim("Email", user.Email),
        //        new Claim("UserName", user.UserName),
        //        new Claim("RoleId", user.RoleId.ToString()),
        //        new Claim(ClaimTypes.Role, MapRoleIdToRoleName(user.RoleId))
        //    };

        //    var principal = new ClaimsPrincipal();

        //    principal.AddIdentity(new ClaimsIdentity(
        //        claims,
        //        CookieAuthenticationDefaults.AuthenticationScheme));

        //    return principal;
        //}

        private static string MapRoleIdToRoleName(long? roleId)
        {
            return roleId switch
            {
                1 => "Administrator",
                2 => "Manager",
                3 => "Supervisor",
                4 => "Pharmacist",
                5 => "StoreKeeper",
                6 => "Distributor",
                7 => "Sales",
                _ => "User"
            };
        }
    }
}