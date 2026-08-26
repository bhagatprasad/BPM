using BPM.Web.API.Models.Extensions;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repositories.Interfaces;
using BPM.Web.Orders.API.Helpers;
using BPM.Web.Orders.API.Models.DTOs;
using BPM.Web.Orders.API.Models.Entities;
using BPM.Web.Orders.API.Models.Mappers;
using BPM.Web.Orders.API.Repository;

namespace BPM.Web.Orders.API.Services
{
  /*  public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly IPurchaseOrderApprovalRepository _approvalRepository;
        private readonly ILogger<PurchaseOrderService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PurchaseOrderService(IPurchaseOrderRepository repository, IPurchaseOrderApprovalRepository approvalRepository, IServiceProvider serviceProvider, ILogger<PurchaseOrderService> logger)
        {
            _repository = repository;
            _approvalRepository = approvalRepository;
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

                var requiredApprovalLevels = GetRequiredApprovalLevels(purchaseOrder.TotalAmount);
                _logger.LogInformation("Purchase Order {OrderId} requires {ApprovalLevels} approval level(s). Total Amount: {TotalAmount}", purchaseOrder.Id, requiredApprovalLevels, purchaseOrder.TotalAmount);

                var approvers = await _approvalRepository.GetActiveApproversAsync();

                if (approvers.Count < requiredApprovalLevels)
                {
                    _logger.LogWarning("Insufficient active approvers for Purchase Order {OrderId}. Required: {Required}, Available: {Available}", purchaseOrder.Id, requiredApprovalLevels, approvers.Count);
                    throw new InvalidOperationException("Insufficient active approvers are available for this Purchase Order.");
                }

                var approvalRecords = new List<PurchaseOrderApproval>();

                for (int level = 1; level <= requiredApprovalLevels; level++)
                {
                    var approver = approvers[level - 1];
                    approvalRecords.Add(new PurchaseOrderApproval
                    {
                        Id = Guid.NewGuid(),
                        PurchaseOrderId = purchaseOrder.Id,
                        ApproverId = approver.Id,
                        ApprovalLevel = level,
                        Status = "Pending",
                        CreatedBy = currentUserId,
                        CreatedOn = DateTime.UtcNow
                    });
                }

                purchaseOrder.Status = "Submitted";
                purchaseOrder.ModifiedBy = currentUserId;
                purchaseOrder.ModifiedOn = DateTime.UtcNow;
                purchaseOrder.EnsureAllDateTimesUtc();

                await _approvalRepository.SubmitPurchaseOrderWithApprovalsAsync(purchaseOrder, approvalRecords);

                _logger.LogInformation("Purchase Order submitted successfully. OrderId: {OrderId}, PONumber: {PONumber}", purchaseOrder.Id, purchaseOrder.PONumber);

                var result = await _repository.GetPurchaseOrderByIdAsync(purchaseOrder.Id);

                if (result == null)
                {
                    throw new InvalidOperationException("Purchase Order was submitted but could not be retrieved.");
                }

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

        public async Task<PurchaseOrderResponseDto> CopyPurchaseOrderAsync(Guid purchaseOrderId, Guid currentUserId)
        {
            try
            {
                // Log the start of the Purchase Order copy operation.
                _logger.LogInformation("Copying Purchase Order. Source OrderId: {OrderId}", purchaseOrderId);

                // Fetch the existing Purchase Order with its items, drugs, and packaging details.
                var sourcePurchaseOrder = await _repository.GetPurchaseOrderByIdAsync(purchaseOrderId);

                // Validate that the source Purchase Order exists.
                if (sourcePurchaseOrder == null)
                {
                    _logger.LogWarning("Purchase Order not found. OrderId: {OrderId}", purchaseOrderId);
                    throw new InvalidOperationException("Purchase Order not found.");
                }

                // Only Completed Purchase Orders are allowed to be copied.
                if (!string.Equals(sourcePurchaseOrder.Status, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Purchase Order cannot be copied because its status is {Status}. OrderId: {OrderId}", sourcePurchaseOrder.Status, purchaseOrderId);
                    throw new InvalidOperationException("Only Completed Purchase Orders can be copied.");
                }

                // Validate that the source Purchase Order contains at least one item.
                if (sourcePurchaseOrder.PurchaseOrderItems == null || !sourcePurchaseOrder.PurchaseOrderItems.Any())
                {
                    throw new InvalidOperationException("Purchase Order must contain at least one item.");
                }

                // Create a new Purchase Order as Draft.
                var newPurchaseOrder = new PurchaseOrder
                {
                    // Generate a new unique Purchase Order Id.
                    Id = Guid.NewGuid(),

                    // Generate a new Purchase Order number.
                    PONumber = $"PO-{DateTime.UtcNow:yyyyMM}-{DateTime.UtcNow.Ticks.ToString()[^4..]}",

                    // Copy Supplier and Dealer information from the original Purchase Order.
                    SupplierId = sourcePurchaseOrder.SupplierId,
                    DealerId = sourcePurchaseOrder.DealerId,

                    // Set the new Purchase Order date.
                    OrderDate = DateTime.UtcNow,

                    // Copy expected delivery date from the original Purchase Order.
                    ExpectedDeliveryDate = sourcePurchaseOrder.ExpectedDeliveryDate,

                    // A copied Purchase Order has no actual delivery date.
                    ActualDeliveryDate = null,

                    // Copied Purchase Order always starts as Draft.
                    Status = "Draft",

                    // Copy currency and payment/delivery terms.
                    CurrencyCode = sourcePurchaseOrder.CurrencyCode,
                    PaymentTerms = sourcePurchaseOrder.PaymentTerms,
                    DeliveryTerms = sourcePurchaseOrder.DeliveryTerms,

                    // Maintain reference to the original Purchase Order.
                    Remarks = $"Copied from {sourcePurchaseOrder.PONumber}",

                    // Copy internal notes.
                    InternalNotes = sourcePurchaseOrder.InternalNotes,

                    // New Purchase Order is active.
                    IsActive = true,

                    // Maintain audit information for the copy operation.
                    CreatedBy = currentUserId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = currentUserId,
                    ModifiedOn = DateTime.UtcNow
                };

                // Create a collection to store the copied Purchase Order items.
                var newPurchaseOrderItems = new List<PurchaseOrderItem>();

                // Process each item from the original Purchase Order.
                foreach (var sourceItem in sourcePurchaseOrder.PurchaseOrderItems)
                {
                    // Validate that the Drug is active.
                    if (sourceItem.Drug == null || !sourceItem.Drug.IsActive)
                    {
                        throw new InvalidOperationException($"Drug is inactive or unavailable. DrugId: {sourceItem.DrugId}");
                    }

                    // Validate that the Drug Packaging is active.
                    if (sourceItem.DrugPackaging == null || !sourceItem.DrugPackaging.IsActive)
                    {
                        throw new InvalidOperationException($"Drug packaging is inactive or unavailable. PackagingId: {sourceItem.PackagingId}");
                    }

                    // Validate current inventory availability for the requested quantity.
                    var availability = await _repository.ValidateProductAvailabilityAsync(sourceItem.DrugId, sourceItem.PackagingId, sourceItem.Quantity);

                    // Stop the copy operation when the requested quantity is unavailable.
                    if (!availability.IsAvailable)
                    {
                        throw new InvalidOperationException($"Product {sourceItem.Drug?.DrugName ?? sourceItem.DrugId.ToString()} is not available in the requested quantity. {availability.Message}");
                    }

                    // Get the current packaging price instead of using the historical PO price.
                    var currentUnitPrice = sourceItem.DrugPackaging.PackagePrice;

                    // Get the current applicable discount based on supplier, product, packaging, and quantity.
                    var currentDiscountPercentage = await _repository.GetCurrentDiscountPercentageAsync(sourcePurchaseOrder.SupplierId, sourceItem.DrugId, sourceItem.PackagingId, sourceItem.Quantity);

                    // Create a new Purchase Order item.
                    var newItem = new PurchaseOrderItem
                    {
                        // Generate a new item Id.
                        Id = Guid.NewGuid(),

                        // Copy Drug and Packaging information.
                        DrugId = sourceItem.DrugId,
                        PackagingId = sourceItem.PackagingId,

                        // Copy the requested quantity.
                        Quantity = sourceItem.Quantity,

                        // Apply the current price.
                        UnitPrice = currentUnitPrice,

                        // Discount will be recalculated using current offers.
                        DiscountPercentage = currentDiscountPercentage,
                        DiscountAmount = 0,

                        // Copy the tax rate.
                        TaxRate = sourceItem.TaxRate,

                        // Recalculate tax and total amount.
                        TaxAmount = 0,
                        TotalAmount = 0,

                        // Reset receiving information for the new Draft PO.
                        ReceivedQuantity = 0,
                        PendingQuantity = sourceItem.Quantity,

                        // Batch information belongs to the previous order and should not be copied.
                        BatchNumber = null,
                        ExpiryDate = null,

                        // Copy item remarks.
                        Remarks = sourceItem.Remarks,

                        // Maintain audit information.
                        CreatedBy = currentUserId,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = currentUserId,
                        ModifiedOn = DateTime.UtcNow
                    };

                    // Calculate the new item subtotal using the current price.
                    var subTotal = newItem.UnitPrice * newItem.Quantity;

                    // Calculate the current discount amount.
                    newItem.DiscountAmount = subTotal * newItem.DiscountPercentage / 100;

                    // Calculate the amount after applying the current discount.
                    var amountAfterDiscount = subTotal - newItem.DiscountAmount;

                    // Calculate tax after applying the discount.
                    newItem.TaxAmount = amountAfterDiscount * newItem.TaxRate / 100;

                    // Calculate the final item total.
                    newItem.TotalAmount = amountAfterDiscount + newItem.TaxAmount;

                    // Add the new item to the copied Purchase Order.
                    newPurchaseOrderItems.Add(newItem);
                }

                // Calculate the new Purchase Order subtotal.
                newPurchaseOrder.SubTotal = newPurchaseOrderItems.Sum(x => x.UnitPrice * x.Quantity);

                // Calculate the total discount for the new Purchase Order.
                newPurchaseOrder.DiscountAmount = newPurchaseOrderItems.Sum(x => x.DiscountAmount);

                // Calculate the total tax for the new Purchase Order.
                newPurchaseOrder.TaxAmount = newPurchaseOrderItems.Sum(x => x.TaxAmount);

                // Calculate the final total amount for the new Purchase Order.
                newPurchaseOrder.TotalAmount = newPurchaseOrderItems.Sum(x => x.TotalAmount);

                // Ensure all Purchase Order DateTime values are stored in UTC.
                newPurchaseOrder.EnsureAllDateTimesUtc();

                // Save the new Purchase Order and its copied items.
                var result = await _repository.CreatePurchaseOrderAsync(newPurchaseOrder, newPurchaseOrderItems);

                // Log successful completion of the copy operation.
                _logger.LogInformation("Purchase Order copied successfully. Source PO: {SourcePONumber}, New PO: {NewPONumber}", sourcePurchaseOrder.PONumber, result.PONumber);

                // Fetch the newly created Purchase Order with its related data.
                var dbPurchaseOrder = await _repository.GetPurchaseOrderByIdAsync(result.Id);

                // Convert the new Purchase Order entity to the response DTO.
                return dbPurchaseOrder.ToDto();
            }
            catch (Exception ex)
            {
                // Log any error that occurs during the copy operation.
                _logger.LogError(ex, "Error occurred while copying Purchase Order. Source OrderId: {OrderId}", purchaseOrderId);

                // Pass the exception to the controller/global exception handler.
                throw;
            }
        }

        private int GetRequiredApprovalLevels(decimal totalAmount)
        {
            if (totalAmount <= 50000)
            {
                return 1;
            }

            if (totalAmount <= 500000)
            {
                return 2;
            }

            return 3;
        }
    }*/
}
