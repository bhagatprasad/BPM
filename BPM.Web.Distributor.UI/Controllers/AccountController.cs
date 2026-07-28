using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new AuthenticateUserDto
            {
                Username = model.Username,
                Password = model.Password
            };

            var result = await _service.LoginAsync(dto);

            if (result == null)
            {
                TempData["Error"] = "Unable to connect to server.";
                return View(model);
            }

            if (!result.IsValidUser)
            {
                TempData["Error"] = "Invalid Username.";
                return View(model);
            }

            if (!result.IsValidPassword)
            {
                TempData["Error"] = "Invalid Password.";
                return View(model);
            }

            HttpContext.Session.SetString("JwtToken", result.JwtToken);
            HttpContext.Session.SetString("UserName", result.AuthenticateResponseDto.FirstName);

            TempData["Success"] = "Login Successful.";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new ForgotPasswordDto
            {
                Username = model.Username
            };

            var result = await _service.ForgotPasswordAsync(dto);

            if (result == null)
            {
                TempData["Error"] = "Unable to connect to server.";
                return View(model);
            }

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(model);
            }

            return RedirectToAction("ResetPassword", new
            {
                userId = result.UserId
            });
        }

        [HttpGet]
        public IActionResult ResetPassword(Guid userId)
        {
            var model = new ResetPasswordViewModel
            {
                UserId = userId.ToString()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new ResetPasswordDto
            {
                UserId = model.UserId,
                NewPassword = model.NewPassword
            };

            var success = await _service.ResetPasswordAsync(dto);

            if (!success)
            {
                TempData["Error"] = "Password reset failed.";
                return View(model);
            }

            TempData["Success"] = "Password reset successfully.";

            return RedirectToAction(nameof(Login));
        }
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Success"] = "Logged out successfully.";

            return RedirectToAction("Login", "Account");
        }
    }
}