using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    [Authorize(Policy = "DistributorPortal")]
    public class SalesOrderController : Controller
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ILogger<SalesOrderController> _logger;
        private readonly INotyfService _notyf;
        public SalesOrderController(ISalesOrderService salesOrderService, ILogger<SalesOrderController> logger, INotyfService notyf)
        {
            _salesOrderService = salesOrderService;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSalesOrders()
        {
            try
            {
                var salesOrders = await _salesOrderService.GetAllSalesOrderAsync();

                if (salesOrders.Any())
                {
                    return Json(salesOrders.OrderByDescending(x => x.ModifiedOn));
                }

                return Json(salesOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sales orders.");
                _notyf.Error("An error occurred while fetching sales orders.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
