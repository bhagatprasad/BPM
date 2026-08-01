using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BPM.Web.Distributor.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly INotyfService _notyfService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public object JsonConvert { get; private set; }

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
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Clear();

                foreach (var cookie in Request.Cookies.Keys)
                    Response.Cookies.Delete(cookie);

                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserDto model)
        {
            try
            {
                var response = await _authenticateService.AuthenticateUserAsync(model);

                if (response == null)
                {
                    _notyfService.Error("Unable to connect to server.");
                    return Json(new { appUser = default(object) });
                }

                if (!response.IsValidUser)
                {
                    _notyfService.Warning(response.Message);
                    return Json(new { appUser = default(object) });
                }

                if (!response.IsValidPassword)
                {
                    _notyfService.Warning(response.Message);
                    return Json(new { appUser = default(object) });
                }

                // Store Tokens

                HttpContext.Session.SetString("JwtToken", response.JwtToken);
                HttpContext.Session.SetString("RefreshToken", response.RefreshToken);

                // Generate Claims

                var applicationUser =
                    await _authenticateService.GenerateUserClaimsAsync(response);

                HttpContext.Session.SetString(
                 "ApplicationUser",
                 JsonSerializer.Serialize(applicationUser));

                var principal =
                    UserPrincipal.GenerateUserPrincipal(applicationUser);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(8)
                    });

                _notyfService.Success("Login Successful.");

                return Json(new
                {
                    appUser = applicationUser
                });
            }
            catch (Exception ex)
            {
                _notyfService.Error(ex.Message);

                return Json(new
                {
                    appUser = default(object)
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
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto model)
        {
            try
            {
                var response =
                    await _authenticateService.ResetPasswordAsync(model);

                if (response)
                {
                    _notyfService.Success(
                        "Password reset successfully. Please login.");

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

     
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Remove("RefreshToken");
            HttpContext.Session.Remove("ApplicationUser");

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete(".AspNetCore.Cookies");

            _notyfService.Success("Logged out successfully.");

            return RedirectToAction(nameof(Login));
        }

    }
}