using AspNetCoreHero.ToastNotification.Abstractions;
using BPM.Web.Distributor.UI.Models.DTOs;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Distributor.UI.Controllers
{
    [Authorize(Policy = "DistributorPortal")]
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ILogger<PurchaseOrderController> _logger;
        private readonly INotyfService _notyf;
        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, ILogger<PurchaseOrderController> logger, INotyfService notyf)
        {
            _purchaseOrderService = purchaseOrderService;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            try
            {
                var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync();

                if (purchaseOrders.Any())
                {
                    return Json(purchaseOrders.OrderByDescending(x => x.ModifiedOn));
                }

                return Json(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching purchase orders.");
                _notyf.Error("An error occurred while fetching purchase orders.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrdersByDealer(Guid dealerId)
        {
            try
            {
                var purchaseOrders = await _purchaseOrderService.GetPurchaseOrdersByDealerAsync(dealerId);

                return Json(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dealer purchase orders.");
                _notyf.Error("Unable to load purchase orders.");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ProcessPurchaseOrder([FromBody] ProcessPurchaseOrderDto processPurchaseOrderDto)
        {
            try
            {

                var purchaseOrder = await _purchaseOrderService.ProcessPurchaseOrderAsync(processPurchaseOrderDto);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Failed to process purchase order with Id: {Id}", processPurchaseOrderDto.PurchaseOrderId);
                    _notyf.Error("Failed to process purchase order.");
                    return BadRequest("Failed to process purchase order.");
                }

                string message = $"Purchase order processed successfully with Id: {processPurchaseOrderDto.PurchaseOrderId}";
                _logger.LogInformation(message);
                _notyf.Success(message);

                return Json(purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing purchase order with Id: {Id}", processPurchaseOrderDto.PurchaseOrderId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the purchase order.");
            }
        }
    }
}
