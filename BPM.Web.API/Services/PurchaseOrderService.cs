using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Extensions;
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

                // Generate PO Number
                purchaseOrder.PONumber = $"PO-{DateTime.UtcNow:yyyyMM}-{DateTime.UtcNow.Ticks.ToString()[^4..]}";

                // Ensure all DateTime fields are UTC
                purchaseOrder.OrderDate = DateTime.UtcNow;
                purchaseOrder.CreatedOn = DateTime.UtcNow;
                purchaseOrder.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.EnsureUtc();

                foreach (var item in purchaseOrderItems)
                {
                    var subTotal = item.UnitPrice * item.Quantity;

                    item.DiscountAmount = subTotal * item.DiscountPercentage / 100;

                    var amountAfterDiscount = subTotal - item.DiscountAmount;

                    item.TaxAmount = amountAfterDiscount * item.TaxRate / 100;

                    item.TotalAmount = amountAfterDiscount + item.TaxAmount;

                    item.PendingQuantity = item.Quantity;

                    // Ensure ExpiryDate is UTC
                    item.ExpiryDate = item.ExpiryDate.EnsureUtc();
                    item.CreatedOn = DateTime.UtcNow;
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
            try
            {
                _logger.LogInformation("Processing purchase order. OrderId: {OrderId}, Status: {Status}",
                    processPurchaseOrderDto.PurchaseOrderId,
                    processPurchaseOrderDto.Status);

                var purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(
                    processPurchaseOrderDto.PurchaseOrderId);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase order not found. OrderId: {OrderId}",
                        processPurchaseOrderDto.PurchaseOrderId);
                    throw new InvalidOperationException("Purchase order not found.");
                }

                // Validate status transition
                if (!IsValidStatusTransition(purchaseOrder.Status, processPurchaseOrderDto.Status))
                {
                    _logger.LogWarning("Invalid status transition from {CurrentStatus} to {NewStatus}",
                        purchaseOrder.Status,
                        processPurchaseOrderDto.Status);
                    throw new InvalidOperationException($"Invalid status transition from {purchaseOrder.Status} to {processPurchaseOrderDto.Status}");
                }

                // Update the existing entity using the mapper
                purchaseOrder.UpdateFromProcessDto(processPurchaseOrderDto, currentUserId);

                // Ensure all DateTime fields are UTC
                purchaseOrder.EnsureAllDateTimesUtc();

                // Save changes
                var updateOrder = await _repository.UpdatePurchaseOrderAsync(purchaseOrder);

                // If status is Approved or Verified, create sales order
                if (string.Equals(updateOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(updateOrder.Status, "Verified", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var salesOrderService = _serviceProvider.GetRequiredService<ISalesOrderService>();

                        if (salesOrderService != null)
                        {
                            _logger.LogInformation("Creating sales order from purchase order. OrderId: {OrderId}",
                                updateOrder.Id);

                            await salesOrderService.CreateSalesOrderFromPurchaseOrderAsync(
                                updateOrder.Id,
                                updateOrder.ModifiedBy.Value);

                            _logger.LogInformation("Sales order created successfully for Purchase Order: {PONumber}",
                                updateOrder.PONumber);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating sales order from purchase order. OrderId: {OrderId}",
                            updateOrder.Id);
                        // Optionally: You might want to rethrow or handle this differently
                        throw;
                    }
                }

                _logger.LogInformation("Purchase order processed successfully. OrderId: {OrderId}, Status: {Status}",
                    updateOrder.Id,
                    updateOrder.Status);

                return updateOrder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing purchase order. OrderId: {OrderId}",
                    processPurchaseOrderDto.PurchaseOrderId);
                throw;
            }
        }

        /// <summary>
        /// Validates if the status transition is allowed
        /// </summary>
        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            // Define allowed status transitions
            var allowedTransitions = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Draft", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Submitted", "Cancelled" } },
                { "Submitted", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Verified", "Rejected", "Cancelled" } },
                { "Verified", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Approved", "Rejected", "Cancelled" } },
                { "Approved", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Processing", "Cancelled" } },
                { "Processing", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Sent to Inventory", "Cancelled" } },
                { "Sent to Inventory", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Inventory Confirmed", "Partially Available", "Out of Stock" } },
                { "Inventory Confirmed", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Ready for Dispatch", "Partially Available" } },
                { "Partially Available", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Inventory Confirmed", "Out of Stock" } },
                { "Out of Stock", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Ready for Dispatch" } },
                { "Ready for Dispatch", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Dispatched" } },
                { "Dispatched", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "In Transit" } },
                { "In Transit", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Partially Delivered", "Delivered" } },
                { "Partially Delivered", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Delivered" } },
                { "Delivered", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Bill Generated", "Partially Delivered" } },
                { "Bill Generated", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Payment Pending" } },
                { "Payment Pending", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Partially Paid", "Paid", "Payment Failed", "Payment Overdue" } },
                { "Partially Paid", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Paid", "Payment Failed", "Payment Overdue" } },
                { "Paid", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Completed", "Payment Failed" } },
                { "Completed", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Closed" } },
                { "Rejected", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Closed" } },
                { "Cancelled", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Closed" } }
            };

            // If no transitions defined for current status, return false
            if (!allowedTransitions.ContainsKey(currentStatus))
            {
                return false;
            }

            // Check if new status is in allowed transitions
            return allowedTransitions[currentStatus].Contains(newStatus);
        }
    }
}