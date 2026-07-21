using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using BPM.Web.API.Services;

namespace BPM.Web.API.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly ILogger<PurchaseOrderService> _logger;

        public PurchaseOrderService(
            IPurchaseOrderRepository repository,
            ILogger<PurchaseOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto createPurchaseOrderDto)
        {
            try
            {
                _logger.LogInformation("Creating Purchase Order.");

                var purchaseOrder = createPurchaseOrderDto.ToEntity();
                var purchaseOrderItems = createPurchaseOrderDto.Items
                    .Select(x => x.ToEntity())
                    .ToList();

                purchaseOrder.PONumber = $"PO-{DateTime.UtcNow:yyyyMM}-{DateTime.UtcNow.Ticks.ToString()[^4..]}";
                foreach (var item in purchaseOrderItems)
                {
                    var subTotal = item.UnitPrice * item.Quantity;

                    item.DiscountAmount = subTotal * item.DiscountPercentage / 100;

                    var amountAfterDiscount = subTotal - item.DiscountAmount;

                    item.TaxAmount = amountAfterDiscount * item.TaxRate / 100;

                    item.TotalAmount = amountAfterDiscount + item.TaxAmount;

                    item.PendingQuantity = item.Quantity;
                }

                purchaseOrder.SubTotal = purchaseOrderItems.Sum(x => x.UnitPrice * x.Quantity);

                purchaseOrder.DiscountAmount = purchaseOrderItems.Sum(x => x.DiscountAmount);

                purchaseOrder.TaxAmount = purchaseOrderItems.Sum(x => x.TaxAmount);

                purchaseOrder.TotalAmount = purchaseOrderItems.Sum(x => x.TotalAmount);

                var result = await _repository.CreatePurchaseOrderAsync(purchaseOrder, purchaseOrderItems);

                _logger.LogInformation("Purchase Order created successfully. PO Number: {PONumber}", result.PONumber);

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Purchase Order.");
                throw;
            }
        }

        public async Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all purchase orders.");

                var purchaseOrders = await _repository.GetPurchaseOrdersAllAsync();

                if (!purchaseOrders.Any())
                {
                    _logger.LogWarning("No purchase orders found.");
                    return Enumerable.Empty<PurchaseOrderResponseDto>();
                }

                return purchaseOrders.Select(po => po.ToDto()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase orders.");
                throw;
            }
        }

        public async Task<PurchaseOrderResponseDto?> GetPurchaseOrderByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching purchase order with Id: {Id}", id);

                var purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(id);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase order not found with Id: {Id}", id);
                    return null;
                }

                return purchaseOrder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase order with Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByDealerAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching purchase orders for Dealer Id: {DealerId}", dealerId);

                var purchaseOrders = await _repository.GetPurchaseOrdersByDealerAsync(dealerId);

                if (!purchaseOrders.Any())
                {
                    _logger.LogWarning("No purchase orders found for Dealer Id: {DealerId}", dealerId);
                    return Enumerable.Empty<PurchaseOrderResponseDto>();
                }

                return purchaseOrders.Select(po => po.ToDto()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching purchase orders for Dealer Id: {DealerId}", dealerId);
                throw;
            }
        }
    }
}