using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BPM.Web.API.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ILogger<SalesOrderService> _logger;

        public SalesOrderService(ISalesOrderRepository salesOrderRepository, ILogger<SalesOrderService> logger)
        {
            _salesOrderRepository = salesOrderRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync()
        {
            try
            {
                _logger.LogInformation("Getting all sales orders");
                var salesorders = await _salesOrderRepository.GetAllSalesOrderAsync();
                return salesorders.Select(SalesOrderMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all sales orders");
                throw;
            }
        }

        public async Task<IEnumerable<SalesOrderDto>> GetSalesOrderByDealerAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Getting sales orders for DealerId: {DealerId}", dealerId);
                var salesorder = await _salesOrderRepository.GetSalesOrderByDealer(dealerId);
                return salesorder.Select(SalesOrderMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occurred while getting sales order for DealerId : {dealerId}", dealerId);
                throw;
            }
        }

        public async Task<SalesOrderDto>
           CreateSalesOrderFromPurchaseOrderAsync(Guid purchaseOrderId)
        {
            try
            {
                _logger.LogInformation(
                    "Creating Sales Order from Purchase Order: {PurchaseOrderId}",
                    purchaseOrderId);

                // 1. Get Purchase Order with Items
                var purchaseOrder =
                    await _salesOrderRepository
                        .GetPurchaseOrderWithItemsAsync(purchaseOrderId);

                // 2. Validate Purchase Order exists
                if (purchaseOrder == null)
                {
                    _logger.LogWarning(
                        "Purchase Order not found: {PurchaseOrderId}",
                        purchaseOrderId);

                    throw new KeyNotFoundException(
                        "Purchase Order not found.");
                }

                // 3. Validate Purchase Order status
                if (!string.Equals(
                        purchaseOrder.Status,
                        "Approved",
                        StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "Purchase Order {PurchaseOrderId} is not approved. Current status: {Status}",
                        purchaseOrderId,
                        purchaseOrder.Status);

                    throw new InvalidOperationException(
                        "Only approved Purchase Orders can be converted to Sales Orders.");
                }

                // 4. Validate Purchase Order Items
                if (purchaseOrder.PurchaseOrderItems == null ||
                    !purchaseOrder.PurchaseOrderItems.Any())
                {
                    _logger.LogWarning(
                        "Purchase Order {PurchaseOrderId} has no items",
                        purchaseOrderId);

                    throw new InvalidOperationException(
                        "Purchase Order does not contain any items.");
                }

                // 5. Create Sales Order
                var salesOrder = new SalesOrder
                {
                    Id = Guid.NewGuid(),

                    SONumber =
                        $"SO-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",

                    PurchaseOrderId = purchaseOrder.Id,
                    SupplierId = purchaseOrder.SupplierId,
                    DealerId = purchaseOrder.DealerId,

                    OrderDate = DateTime.UtcNow,

                    ExpectedDeliveryDate =
                        purchaseOrder.ExpectedDeliveryDate,

                    Status = "Created",

                    SubTotal = purchaseOrder.SubTotal,
                    TaxAmount = purchaseOrder.TaxAmount,
                    DiscountAmount = purchaseOrder.DiscountAmount,
                    TotalAmount = purchaseOrder.TotalAmount,

                    CurrencyCode = purchaseOrder.CurrencyCode,

                    PaymentTerms = purchaseOrder.PaymentTerms,
                    DeliveryTerms = purchaseOrder.DeliveryTerms,
                    Remarks = purchaseOrder.Remarks,
                    InternalNotes = purchaseOrder.InternalNotes,

                    IsActive = true,

                    CreatedBy = purchaseOrder.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                // 6. Convert PurchaseOrderItems → SalesOrderItems
                var salesOrderItems =
                    purchaseOrder.PurchaseOrderItems
                        .Select(item =>
                            item.ToSalesOrderItem(salesOrder.Id))
                        .ToList();

                // 7. Save SalesOrder + SalesOrderItems
                var createdSalesOrder =
                    await _salesOrderRepository
                        .CreateSalesOrderAsync(
                            salesOrder,
                            salesOrderItems);

                _logger.LogInformation(
                    "Sales Order {SONumber} created successfully from Purchase Order {PurchaseOrderId}",
                    createdSalesOrder.SONumber,
                    purchaseOrderId);

                // 8. Return DTO
                return createdSalesOrder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating Sales Order from Purchase Order: {PurchaseOrderId}",
                    purchaseOrderId);

                throw;
            }
        }
    }
}
