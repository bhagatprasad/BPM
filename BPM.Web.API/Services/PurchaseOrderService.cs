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
        private readonly IServiceProvider _serviceProvider;

        public PurchaseOrderService(
            IPurchaseOrderRepository repository,
            IServiceProvider serviceProvider,
            ILogger<PurchaseOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
            _serviceProvider = serviceProvider;
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

        public async Task<PurchaseOrderResponseDto> ProcessPurchaseOrderAsync(ProcessPurchaseOrderDto processPurchaseOrderDto, Guid currentUserId)
        {
            var purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(processPurchaseOrderDto.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                throw new InvalidOperationException("Purchase order not found.");
            }


            //step 1 -- if the status == approved then we need to check if the actual delivery date is set or not if not then we need to set it to the current date and time

            // creat a sales order from the purchase order and set the status to processed and set the processed date to the current date and time

            var updateOrder = await _repository.UpdatePurchaseOrderAsync(processPurchaseOrderDto.ToPurchaseOrderFromProcessPurchaseOrderDto(purchaseOrder, currentUserId));

            if (string.Equals(updateOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase))
            {
                // update the status to purcahse ordertable 

                var _salesOrderService = _serviceProvider.GetRequiredService<ISalesOrderService>();

                if (_salesOrderService != null)
                {
                    await _salesOrderService.CreateSalesOrderFromPurchaseOrderAsync(updateOrder.Id, updateOrder.ModifiedBy.Value);
                }

                // create sales order and sales order itsm 

                return updateOrder.ToDto();
            }

            // retrun the updated one 

            return purchaseOrder.ToDto();

        }
    }
}