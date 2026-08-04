using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
   // [Authorize(Policy = "DistributorPortal")]
    public class DealerPurchaseOrderController : Controller
    {
        private readonly IDealerService _dealerService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ILogger<DealerPurchaseOrderController> _logger;
        private readonly INotyfService _notyf;

        public DealerPurchaseOrderController(
            IDealerService dealerService,
            IPurchaseOrderService purchaseOrderService,
            ILogger<DealerPurchaseOrderController> logger,
            INotyfService notyf)
        {
            _dealerService = dealerService;
            _purchaseOrderService = purchaseOrderService;
            _logger = logger;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDealers()
        {
            try
            {
                var dealers = await _dealerService.GetAllDealersAsync();
                return Json(dealers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to fetch dealers.");
                _notyf.Error("Unable to load dealers.");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders(Guid dealerId)
        {
            try
            {
                var orders = await _purchaseOrderService.GetPurchaseOrdersByDealerAsync(dealerId);
                return Json(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to fetch purchase orders.");
                _notyf.Error("Unable to load purchase orders.");
                return StatusCode(500);
            }
        }
    }
}