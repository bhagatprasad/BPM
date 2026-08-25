using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BPM.Web.Identity.API.CustomFilters
{
    [AttributeUsage(AttributeTargets.All | AttributeTargets.Method)]
    public class BPMAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                StringValues authTokens;

                filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);

                var _token = authTokens.FirstOrDefault();

                if (!string.IsNullOrEmpty(_token))
                {
                    // Remove Bearer if present
                    string authToken = _token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();

                    if (IsValidToken(authToken))
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(authToken);

                        // Create ClaimsPrincipal
                        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                        filterContext.HttpContext.User = new ClaimsPrincipal(identity);

                        // Store User Information in HttpContext.Items with fallback claims
                        filterContext.HttpContext.Items["AccessToken"] = authToken;
                        filterContext.HttpContext.Items["UserId"] = jwtToken.Claims
                            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier || x.Type == JwtRegisteredClaimNames.Sub)?.Value;
                        filterContext.HttpContext.Items["Email"] = jwtToken.Claims
                            .FirstOrDefault(x => x.Type == ClaimTypes.Email || x.Type == JwtRegisteredClaimNames.Email)?.Value;
                        filterContext.HttpContext.Items["Role"] = jwtToken.Claims
                            .FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                        filterContext.HttpContext.Items["Name"] = jwtToken.Claims
                            .FirstOrDefault(x => x.Type == ClaimTypes.Name || x.Type == JwtRegisteredClaimNames.Name)?.Value;

                        filterContext.HttpContext.Response.Headers.Add("Authorization", authToken);
                        filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                        filterContext.HttpContext.Response.Headers.Add("StoreAccessibility", "Authorized");

                        return;
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Headers.Add("Authorization", authToken);
                        filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Unauthorized");

                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        var responseFeature = filterContext.HttpContext.Features.Get<IHttpResponseFeature>();
                        if (responseFeature != null)
                        {
                            responseFeature.ReasonPhrase = "Unauthorized";
                        }

                        filterContext.Result = new JsonResult(new
                        {
                            Status = "Error",
                            Message = "Invalid or Expired Token"
                        });

                        return;
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                    var responseFeature = filterContext.HttpContext.Features.Get<IHttpResponseFeature>();
                    if (responseFeature != null)
                    {
                        responseFeature.ReasonPhrase = "Authorization Token Required";
                    }

                    filterContext.Result = new JsonResult(new
                    {
                        Status = "Error",
                        Message = "Please Provide Authorization Token"
                    });

                    return;
                }
            }
        }

        public bool IsValidToken(string authToken)
        {
            return CheckTokenIsValid(authToken);
        }

        public bool CheckTokenIsValid(string token)
        {
            try
            {
                var tokenTicks = GetTokenExpirationTime(token);

                var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

                var now = DateTime.UtcNow;

                return tokenDate >= now;
            }
            catch
            {
                return false;
            }
        }

        public long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenExp = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type.Equals("exp") || claim.Type.Equals(JwtRegisteredClaimNames.Exp))?
                .Value;

            if (string.IsNullOrEmpty(tokenExp))
                throw new Exception("Token expiration claim not found");

            return long.Parse(tokenExp);
        }
    }
}