using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BPM.Web.Distributor.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly INotyfService _notyfService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(
            IAuthenticateService authenticateService,
            INotyfService notyfService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticateService = authenticateService;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // Check if user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page instead of clearing session
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Clear session
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Remove("RefreshToken");
            HttpContext.Session.Remove("AuthResponse");

            // Sign out
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Delete cookies
            Response.Cookies.Delete(".AspNetCore.Cookies");

            // Clear all cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            _notyfService.Success("Logged out successfully.");

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserDto model)
        {
            try
            {
                var response = await _authenticateService.AuthenticateUserAsync(model);

                if (!string.IsNullOrEmpty(response.JwtToken))
                {
                    // Check if user has dealer or is Admin/Operator
                    var roleName = response.AuthenticateResponseDto?.RoleInfo?.Name;
                    var dealerInfo = response.AuthenticateResponseDto?.DealerInfo;

                    // Check if user is Administrator or Operator
                    bool isAdminOrOperator = roleName == "Administrator" || roleName == "Operator";

                    // Allow login if:
                    // 1. User has dealer info, OR
                    // 2. User is Administrator or Operator (can login without dealer)
                    if (dealerInfo == null && !isAdminOrOperator)
                    {
                        // User doesn't have dealer and is not Admin/Operator - deny access
                        var errorMsg = "You are not authorized to login to this portal. Please use the dealer portal to login.";
                        _notyfService.Error(errorMsg);

                        return Json(new
                        {
                            appUser = response,
                            hasAccess = false,
                            message = errorMsg
                        });
                    }

                    // Store session data
                    _httpContextAccessor.HttpContext.Session.SetString("JwtToken", response.JwtToken);
                    _httpContextAccessor.HttpContext.Session.SetString("RefreshToken", response.RefreshToken);
                    _httpContextAccessor.HttpContext.Session.SetString("AuthResponse", JsonConvert.SerializeObject(response));

                    // Generate user principal
                    var principal = UserPrincipal.GenerateUserPrincipal(response);

                    // Sign in user
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(8)
                        });

                    var successMsg = "Login Successful.";
                    _notyfService.Success(successMsg);

                    return Json(new
                    {
                        appUser = response,
                        hasAccess = true,
                        message = successMsg,
                        redirectUrl = Url.Action("Index", "Home")
                    });
                }
                else
                {
                    var errorMsg = response.Message ?? "Login failed. Please check your credentials.";
                    _notyfService.Error(errorMsg);

                    return Json(new
                    {
                        appUser = response,
                        hasAccess = false,
                        message = errorMsg
                    });
                }
            }
            catch (Exception ex)
            {
                var errorMsg = "An error occurred during login. Please try again.";
                _notyfService.Error(errorMsg);

                return Json(new
                {
                    appUser = default(object),
                    hasAccess = false,
                    message = errorMsg
                });
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            try
            {
                var response = await _authenticateService.ForgotPasswordAsync(model);

                if (response == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to process request."
                    });
                }

                return Json(new
                {
                    success = response.Success,
                    message = response.Message,
                    userId = response.UserId
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(Guid userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            try
            {
                var response = await _authenticateService.ResetPasswordAsync(model);

                if (response)
                {
                    _notyfService.Success("Password reset successfully. Please login.");
                    return Json(new
                    {
                        success = true
                    });
                }

                _notyfService.Warning("Unable to reset password.");
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception ex)
            {
                _notyfService.Error(ex.Message);
                return Json(new
                {
                    success = false
                });
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}