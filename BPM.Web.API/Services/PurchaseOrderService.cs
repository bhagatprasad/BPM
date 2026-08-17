using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;
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

        public PurchaseOrderService(IPurchaseOrderRepository repository, IServiceProvider serviceProvider, ILogger<PurchaseOrderService> logger)
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
                var purchaseOrderItems = createPurchaseOrderDto.Items.Select(x => x.ToEntity()).ToList();
                purchaseOrder.PONumber = $"PO-{DateTime.UtcNow:yyyyMM}-{DateTime.UtcNow.Ticks.ToString()[^4..]}";
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
                    item.ExpiryDate = item.ExpiryDate.EnsureUtc();
                    item.CreatedOn = DateTime.UtcNow;
                }

                purchaseOrder.SubTotal = purchaseOrderItems.Sum(x => x.UnitPrice * x.Quantity);
                purchaseOrder.DiscountAmount = purchaseOrderItems.Sum(x => x.DiscountAmount);
                purchaseOrder.TaxAmount = purchaseOrderItems.Sum(x => x.TaxAmount);
                purchaseOrder.TotalAmount = purchaseOrderItems.Sum(x => x.TotalAmount);
                var result = await _repository.CreatePurchaseOrderAsync(purchaseOrder, purchaseOrderItems);
                _logger.LogInformation("Purchase Order created successfully. PO Number: {PONumber}", result.PONumber);

                if (result != null)
                {
                    var dbPurchaseOrder = await _repository.GetPurchaseOrderByIdAsync(purchaseOrder.Id);
                    return dbPurchaseOrder.ToDto();
                }

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
                _logger.LogInformation("Processing purchase order. OrderId: {OrderId}, Status: {Status}", processPurchaseOrderDto.PurchaseOrderId, processPurchaseOrderDto.Status);
                var purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(processPurchaseOrderDto.PurchaseOrderId);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase order not found. OrderId: {OrderId}", processPurchaseOrderDto.PurchaseOrderId);
                    throw new InvalidOperationException("Purchase order not found.");
                }

                if (!IsValidStatusTransition(purchaseOrder.Status, processPurchaseOrderDto.Status))
                {
                    _logger.LogWarning("Invalid status transition from {CurrentStatus} to {NewStatus}", purchaseOrder.Status, processPurchaseOrderDto.Status);
                    throw new InvalidOperationException($"Invalid status transition from {purchaseOrder.Status} to {processPurchaseOrderDto.Status}");
                }

                purchaseOrder.UpdateFromProcessDto(processPurchaseOrderDto, currentUserId);
                purchaseOrder.EnsureAllDateTimesUtc();
                var updateOrder = await _repository.UpdatePurchaseOrderAsync(purchaseOrder);

                if (string.Equals(updateOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase) || string.Equals(updateOrder.Status, "Verified", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var salesOrderService = _serviceProvider.GetRequiredService<ISalesOrderService>();

                        if (salesOrderService != null)
                        {
                            _logger.LogInformation("Creating sales order from purchase order. OrderId: {OrderId}", updateOrder.Id);
                            await salesOrderService.CreateSalesOrderFromPurchaseOrderAsync(updateOrder.Id, updateOrder.ModifiedBy.Value);
                            _logger.LogInformation("Sales order created successfully for Purchase Order: {PONumber}", updateOrder.PONumber);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating sales order from purchase order. OrderId: {OrderId}", updateOrder.Id);
                        throw;
                    }
                }

                _logger.LogInformation("Purchase order processed successfully. OrderId: {OrderId}, Status: {Status}", updateOrder.Id, updateOrder.Status);
                return updateOrder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing purchase order. OrderId: {OrderId}", processPurchaseOrderDto.PurchaseOrderId);
                throw;
            }
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
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

            if (!allowedTransitions.ContainsKey(currentStatus))
            {
                return false;
            }

            return allowedTransitions[currentStatus].Contains(newStatus);
        }

        public async Task<ProductAvailabilityResponseDto> ValidateProductAvailabilityAsync(Guid drugId, Guid packagingId, int quantity)
        {
            try
            {
                _logger.LogInformation("Validating product availability for DrugId: {DrugId}, PackagingId: {PackagingId}, Quantity: {Quantity}", drugId, packagingId, quantity);

                if (drugId == Guid.Empty)
                {
                    _logger.LogWarning("DrugId is required.");
                    throw new ArgumentException("DrugId is required.");
                }

                if (packagingId == Guid.Empty)
                {
                    _logger.LogWarning("PackagingId is required.");
                    throw new ArgumentException("PackagingId is required.");
                }

                if (quantity <= 0)
                {
                    _logger.LogWarning("Requested quantity must be greater than zero.");
                    throw new ArgumentException("Quantity must be greater than zero.");
                }

                var result = await _repository.ValidateProductAvailabilityAsync(drugId, packagingId, quantity);

                if (!result.IsAvailable)
                {
                    _logger.LogWarning("Product is not available. DrugId: {DrugId}, PackagingId: {PackagingId}, RequestedQuantity: {Quantity}, AvailableQuantity: {AvailableQuantity}", drugId, packagingId, quantity, result.AvailableQuantity);
                }
                else
                {
                    _logger.LogInformation("Product is available. DrugId: {DrugId}, PackagingId: {PackagingId}, RequestedQuantity: {Quantity}, AvailableQuantity: {AvailableQuantity}", drugId, packagingId, quantity, result.AvailableQuantity);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating product availability for DrugId: {DrugId}, PackagingId: {PackagingId}", drugId, packagingId);
                throw;
            }
        }

        public async Task<PurchaseOrderResponseDto> SubmitPurchaseOrderAsync(SubmitPurchaseOrderDto dto, Guid currentUserId)
        {
            try
            {
                _logger.LogInformation("Submitting Purchase Order. OrderId: {OrderId}", dto.PurchaseOrderId);
                var purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(dto.PurchaseOrderId);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase Order not found. OrderId: {OrderId}", dto.PurchaseOrderId);
                    throw new InvalidOperationException("Purchase Order not found.");
                }

                if (!string.Equals(purchaseOrder.Status, "Draft", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Purchase Order cannot be submitted because current status is {Status}. OrderId: {OrderId}", purchaseOrder.Status, purchaseOrder.Id);
                    throw new InvalidOperationException($"Purchase Order can be submitted only from Draft status. Current status: {purchaseOrder.Status}");
                }

                if (purchaseOrder.PurchaseOrderItems == null || !purchaseOrder.PurchaseOrderItems.Any())
                {
                    throw new InvalidOperationException("Purchase Order must contain at least one item.");
                }

                if (purchaseOrder.PurchaseOrderItems.Any(x => x.Quantity <= 0))
                {
                    throw new InvalidOperationException("All Purchase Order item quantities must be greater than zero.");
                }

                if (purchaseOrder.TotalAmount <= 0)
                {
                    throw new InvalidOperationException("Purchase Order total amount must be greater than zero.");
                }

                if (purchaseOrder.ExpectedDeliveryDate < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Expected delivery date must be in the future.");
                }

                purchaseOrder.Status = "Submitted";
                purchaseOrder.ModifiedBy = currentUserId;
                purchaseOrder.ModifiedOn = DateTime.UtcNow;
                purchaseOrder.EnsureAllDateTimesUtc();
                var result = await _repository.SubmitPurchaseOrderAsync(purchaseOrder);
                _logger.LogInformation("Purchase Order submitted successfully. OrderId: {OrderId}, PONumber: {PONumber}", result.Id, result.PONumber);
                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting Purchase Order. OrderId: {OrderId}", dto.PurchaseOrderId);
                throw;
            }
        }

        public async Task<PurchaseOrderResponseDto> SavePurchaseOrderDraftAsync(SavePurchaseOrderDraftDto dto, Guid currentUserId)
        {
            try
            {
                _logger.LogInformation("Saving Purchase Order as Draft. OrderId: {OrderId}", dto.PurchaseOrderId);

                PurchaseOrder purchaseOrder;

                if (dto.PurchaseOrderId.HasValue)
                {
                    purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(dto.PurchaseOrderId.Value);

                    if (purchaseOrder == null)
                    {
                        _logger.LogWarning("Purchase Order not found. OrderId: {OrderId}", dto.PurchaseOrderId);
                        throw new InvalidOperationException("Purchase Order not found.");
                    }

                    if (!string.Equals(purchaseOrder.Status, "Draft", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("Only Draft Purchase Orders can be updated.");
                    }

                    purchaseOrder.SupplierId = dto.SupplierId ?? purchaseOrder.SupplierId;
                    purchaseOrder.DealerId = dto.DealerId ?? purchaseOrder.DealerId;
                    purchaseOrder.ExpectedDeliveryDate = dto.ExpectedDeliveryDate?.EnsureUtc() ?? purchaseOrder.ExpectedDeliveryDate;
                    purchaseOrder.PaymentTerms = dto.PaymentTerms ?? purchaseOrder.PaymentTerms;
                    purchaseOrder.DeliveryTerms = dto.DeliveryTerms ?? purchaseOrder.DeliveryTerms;
                    purchaseOrder.Remarks = dto.Remarks ?? purchaseOrder.Remarks;
                    purchaseOrder.InternalNotes = dto.InternalNotes ?? purchaseOrder.InternalNotes;
                    purchaseOrder.Status = "Draft";
                    purchaseOrder.ModifiedBy = currentUserId;
                    purchaseOrder.ModifiedOn = DateTime.UtcNow;
                }
                else
                {
                    if (!dto.DealerId.HasValue || dto.DealerId == Guid.Empty)
                    {
                        throw new InvalidOperationException("DealerId is required to save a Draft Purchase Order.");
                    }
                    var draftCount = await _repository.GetActiveDraftCountAsync(dto.DealerId.Value);
                    if (draftCount >= 50)
                    {
                        _logger.LogWarning("Maximum draft limit reached for Dealer Id: {DealerId}", dto.DealerId);
                        throw new InvalidOperationException("Maximum 50 active Draft Purchase Orders are allowed.");
                    }
                    purchaseOrder = new PurchaseOrder
                    {
                        PONumber = $"PO-{DateTime.UtcNow:yyyyMM}-{DateTime.UtcNow.Ticks.ToString()[^4..]}",
                        OrderDate = DateTime.UtcNow,
                        SupplierId = dto.SupplierId ?? Guid.Empty,
                        DealerId = dto.DealerId ?? Guid.Empty,
                        ExpectedDeliveryDate = dto.ExpectedDeliveryDate?.EnsureUtc() ?? DateTime.UtcNow.AddDays(7),
                        PaymentTerms = dto.PaymentTerms,
                        DeliveryTerms = dto.DeliveryTerms,
                        Remarks = dto.Remarks,
                        InternalNotes = dto.InternalNotes,
                        Status = "Draft",
                        CurrencyCode = "INR",
                        IsActive = true,
                        CreatedBy = currentUserId,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = currentUserId,
                        ModifiedOn = DateTime.UtcNow
                    };
                }

                var purchaseOrderItems = dto.Items.Select(x => x.ToEntity()).ToList();

                foreach (var item in purchaseOrderItems)
                {
                    var subTotal = item.UnitPrice * item.Quantity;
                    item.DiscountAmount = subTotal * item.DiscountPercentage / 100;
                    var amountAfterDiscount = subTotal - item.DiscountAmount;
                    item.TaxAmount = amountAfterDiscount * item.TaxRate / 100;
                    item.TotalAmount = amountAfterDiscount + item.TaxAmount;
                    item.PendingQuantity = item.Quantity;
                    item.CreatedOn = DateTime.UtcNow;
                }

                purchaseOrder.SubTotal = purchaseOrderItems.Sum(x => x.UnitPrice * x.Quantity);
                purchaseOrder.DiscountAmount = purchaseOrderItems.Sum(x => x.DiscountAmount);
                purchaseOrder.TaxAmount = purchaseOrderItems.Sum(x => x.TaxAmount);
                purchaseOrder.TotalAmount = purchaseOrderItems.Sum(x => x.TotalAmount);
                purchaseOrder.EnsureAllDateTimesUtc();

                var result = await _repository.SavePurchaseOrderDraftAsync(purchaseOrder, purchaseOrderItems);

                _logger.LogInformation("Purchase Order saved as Draft successfully. OrderId: {OrderId}, PONumber: {PONumber}", result.Id, result.PONumber);

                var dbPurchaseOrder = await _repository.GetPurchaseOrderByIdAsync(result.Id);
                return dbPurchaseOrder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving Purchase Order as Draft. OrderId: {OrderId}", dto.PurchaseOrderId);
                throw;
            }
        }

        public async Task<IEnumerable<PurchaseOrderResponseDto>> GetDraftPurchaseOrdersAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching draft purchase orders for Dealer Id: {DealerId}", dealerId);
                var purchaseOrders = await _repository.GetDraftPurchaseOrdersAsync(dealerId);

                if (!purchaseOrders.Any())
                {
                    _logger.LogWarning("No draft purchase orders found for Dealer Id: {DealerId}", dealerId);
                    return Enumerable.Empty<PurchaseOrderResponseDto>();
                }

                return purchaseOrders.Select(po => po.ToDto()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching draft purchase orders for Dealer Id: {DealerId}", dealerId);
                throw;
            }
        }

        public async Task<bool> DeletePurchaseOrderDraftAsync(Guid purchaseOrderId, Guid currentUserId)
        {
            try
            {
                _logger.LogInformation("Deleting Draft Purchase Order. OrderId: {OrderId}", purchaseOrderId);

                var purchaseOrder = await _repository.GetPurchaseOrderByIdAsync(purchaseOrderId);

                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Draft Purchase Order not found. OrderId: {OrderId}", purchaseOrderId);
                    throw new InvalidOperationException("Draft Purchase Order not found.");
                }

                if (!string.Equals(purchaseOrder.Status, "Draft", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Purchase Order is not a Draft. OrderId: {OrderId}", purchaseOrderId);
                    throw new InvalidOperationException("Only Draft Purchase Orders can be deleted.");
                }

                purchaseOrder.ModifiedBy = currentUserId;
                purchaseOrder.ModifiedOn = DateTime.UtcNow;

                var result = await _repository.DeletePurchaseOrderDraftAsync(purchaseOrderId);

                _logger.LogInformation("Draft Purchase Order deleted successfully. OrderId: {OrderId}", purchaseOrderId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Draft Purchase Order. OrderId: {OrderId}", purchaseOrderId);
                throw;
            }
        }

        public async Task<int> DeleteExpiredDraftPurchaseOrdersAsync()
        {
            try
            {
                _logger.LogInformation("Deleting Draft Purchase Orders older than 30 days.");
                var deletedCount = await _repository.DeleteExpiredDraftPurchaseOrdersAsync();
                _logger.LogInformation("Expired Draft Purchase Orders deleted. Count: {Count}", deletedCount);
                return deletedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting expired Draft Purchase Orders.");
                throw;
            }
        }
    }
}