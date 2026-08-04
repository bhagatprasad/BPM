using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    [Authorize(Policy = "DistributorPortal")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly INotyfService _notyfService;

        public UserController(ILogger<UserController> logger, IUserService userService, INotyfService notyfService)
        {
            _logger = logger;
            _userService = userService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersList()
        {
            try
            {
                var users = await _userService.GetAllUsersListAsync();
                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}