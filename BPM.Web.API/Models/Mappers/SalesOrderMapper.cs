using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Extensions;

namespace BPM.Web.API.Models.Mappers
{
    public static class SalesOrderMapper
    {
        public static SalesOrderDto ToDto(this SalesOrder entity)
        {
            return new SalesOrderDto
            {
                Id = entity.Id,
                SONumber = entity.SONumber,
                PurchaseOrderId = entity.PurchaseOrderId,
                SupplierId = entity.SupplierId,
                DealerId = entity.DealerId,
                DistributorId = entity.DistributorId,
                OrderDate = entity.OrderDate,
                ExpectedDeliveryDate = entity.ExpectedDeliveryDate,
                ActualDeliveryDate = entity.ActualDeliveryDate,
                Status = entity.Status,
                SubTotal = entity.SubTotal,
                TaxAmount = entity.TaxAmount,
                DiscountAmount = entity.DiscountAmount,
                TotalAmount = entity.TotalAmount,
                CurrencyCode = entity.CurrencyCode,
                PaymentTerms = entity.PaymentTerms,
                DeliveryTerms = entity.DeliveryTerms,
                Remarks = entity.Remarks,
                InternalNotes = entity.InternalNotes,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                PurchaseOrder = entity.PurchaseOrder.ToDto(),
                SalesOrderItems = entity.SalesOrderItems?.Select(x => x.ToDto()).ToList() ?? new List<SalesOrderItemDto>()
            };
        }

        public static SalesOrderItemDto ToDto(this SalesOrderItem entity)
        {
            return new SalesOrderItemDto
            {
                Id = entity.Id,
                SalesOrderId = entity.SalesOrderId,
                DrugId = entity.DrugId,
                PackagingId = entity.PackagingId,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                DiscountPercentage = entity.DiscountPercentage,
                DiscountAmount = entity.DiscountAmount,
                TaxRate = entity.TaxRate,
                TaxAmount = entity.TaxAmount,
                TotalAmount = entity.TotalAmount,
                ReceivedQuantity = entity.ReceivedQuantity,
                PendingQuantity = entity.PendingQuantity,
                BatchNumber = entity.BatchNumber,
                ExpiryDate = entity.ExpiryDate,
                Remarks = entity.Remarks,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                Drug = entity.Drug?.DrugToDrugDto()
            };
        }

        public static SalesOrder ToSalesOrderFromPurchaseOrder(this PurchaseOrderResponseDto purchaseOrder, Guid createdBy)
        {
            var salesOrder = new SalesOrder
            {
                SONumber = GenerateSONumber(),
                PurchaseOrderId = purchaseOrder.Id,
                SupplierId = purchaseOrder.SupplierId,
                DealerId = purchaseOrder.DealerId,
                DistributorId = purchaseOrder.DistributorId,
                OrderDate = DateTime.UtcNow,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.EnsureUtc(),
                ActualDeliveryDate = purchaseOrder.ActualDeliveryDate.EnsureUtc(),
                Status = "Submitted",
                SubTotal = purchaseOrder.SubTotal,
                TaxAmount = purchaseOrder.TaxAmount,
                DiscountAmount = purchaseOrder.DiscountAmount,
                TotalAmount = purchaseOrder.TotalAmount,
                CurrencyCode = "INR",
                PaymentTerms = purchaseOrder.PaymentTerms,
                DeliveryTerms = purchaseOrder.DeliveryTerms,
                Remarks = purchaseOrder.Remarks,
                InternalNotes = $"Sales order generated from purchase order {purchaseOrder.PONumber} on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
                IsActive = true,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.UtcNow,
                SalesOrderItems = new List<SalesOrderItem>()
            };

            // Map purchase order items to sales order items
            if (purchaseOrder.PurchaseOrderItemResponse != null && purchaseOrder.PurchaseOrderItemResponse.Any())
            {
                salesOrder.SalesOrderItems = purchaseOrder.PurchaseOrderItemResponse
                    .Select(item => item.ToSalesOrderItemFromPurchaseOrderItem())
                    .ToList();
            }

            return salesOrder;
        }

        public static SalesOrder ToSalesOrderFromPurchaseOrderEntity(this PurchaseOrder purchaseOrder, Guid createdBy)
        {
            var salesOrder = new SalesOrder
            {
                SONumber = GenerateSONumber(),
                PurchaseOrderId = purchaseOrder.Id,
                SupplierId = purchaseOrder.SupplierId,
                DealerId = purchaseOrder.DealerId,
                DistributorId = purchaseOrder.DistributorId,
                OrderDate = DateTime.UtcNow,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.EnsureUtc(),
                ActualDeliveryDate = purchaseOrder.ActualDeliveryDate.EnsureUtc(),
                Status = "Submitted",
                SubTotal = purchaseOrder.SubTotal,
                TaxAmount = purchaseOrder.TaxAmount,
                DiscountAmount = purchaseOrder.DiscountAmount,
                TotalAmount = purchaseOrder.TotalAmount,
                CurrencyCode = "INR",
                PaymentTerms = purchaseOrder.PaymentTerms,
                DeliveryTerms = purchaseOrder.DeliveryTerms,
                Remarks = purchaseOrder.Remarks,
                InternalNotes = $"Sales order generated from purchase order {purchaseOrder.PONumber} on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
                IsActive = true,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.UtcNow,
                SalesOrderItems = new List<SalesOrderItem>()
            };

            // Map purchase order items to sales order items
            if (purchaseOrder.PurchaseOrderItems != null && purchaseOrder.PurchaseOrderItems.Any())
            {
                salesOrder.SalesOrderItems = purchaseOrder.PurchaseOrderItems
                    .Select(item => item.ToSalesOrderItemFromPurchaseOrderItem(salesOrder.Id))
                    .ToList();
            }

            return salesOrder;
        }

        public static SalesOrderItem ToSalesOrderItemFromPurchaseOrderItem(this PurchaseOrderItemResponseDto item)
        {
            return new SalesOrderItem
            {
                DrugId = item.DrugId,
                PackagingId = item.PackagingId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountPercentage = item.DiscountPercentage,
                DiscountAmount = item.DiscountAmount,
                TaxRate = item.TaxRate,
                TaxAmount = item.TaxAmount,
                TotalAmount = item.TotalAmount,
                ReceivedQuantity = 0,
                PendingQuantity = item.Quantity,
                BatchNumber = item.BatchNumber,
                ExpiryDate = item.ExpiryDate.EnsureUtc() ?? DateTime.UtcNow.AddYears(1),
                Remarks = item.Remarks,
                CreatedBy = null,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = null,
                ModifiedOn = null
            };
        }

        public static SalesOrderItem ToSalesOrderItemFromPurchaseOrderItem(this PurchaseOrderItem item, Guid salesOrderId)
        {
            return new SalesOrderItem
            {
                SalesOrderId = salesOrderId,
                DrugId = item.DrugId,
                PackagingId = item.PackagingId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountPercentage = item.DiscountPercentage,
                DiscountAmount = item.DiscountAmount,
                TaxRate = item.TaxRate,
                TaxAmount = item.TaxAmount,
                TotalAmount = item.TotalAmount,
                ReceivedQuantity = 0,
                PendingQuantity = item.Quantity,
                BatchNumber = item.BatchNumber,
                ExpiryDate = item.ExpiryDate.EnsureUtc() ?? DateTime.UtcNow.AddYears(1),
                Remarks = item.Remarks,
                CreatedBy = null,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = null,
                ModifiedOn = null
            };
        }

        public static void UpdateSalesOrderFromPurchaseOrder(this SalesOrder salesOrder, PurchaseOrderResponseDto purchaseOrder, Guid modifiedBy)
        {
            salesOrder.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.EnsureUtc();
            salesOrder.ActualDeliveryDate = purchaseOrder.ActualDeliveryDate.EnsureUtc() ?? salesOrder.ActualDeliveryDate;
            salesOrder.SubTotal = purchaseOrder.SubTotal;
            salesOrder.TaxAmount = purchaseOrder.TaxAmount;
            salesOrder.DiscountAmount = purchaseOrder.DiscountAmount;
            salesOrder.TotalAmount = purchaseOrder.TotalAmount;
            salesOrder.PaymentTerms = purchaseOrder.PaymentTerms;
            salesOrder.DeliveryTerms = purchaseOrder.DeliveryTerms;

            if (salesOrder.DistributorId != purchaseOrder.DistributorId)
            {
                salesOrder.DistributorId = purchaseOrder.DistributorId;
            }
            // Append remarks instead of overwriting
            if (!string.IsNullOrWhiteSpace(purchaseOrder.Remarks))
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
                var newNoteEntry = $"[{timestamp}] {purchaseOrder.Remarks}";

                if (string.IsNullOrWhiteSpace(salesOrder.Remarks))
                {
                    salesOrder.Remarks = newNoteEntry;
                }
                else
                {
                    salesOrder.Remarks = $"{salesOrder.Remarks}\n{newNoteEntry}";
                }
            }

            salesOrder.InternalNotes = $"Sales order updated from purchase order {purchaseOrder.PONumber} on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC";
            salesOrder.ModifiedBy = modifiedBy;
            salesOrder.ModifiedOn = DateTime.UtcNow;
        }

        public static void UpdateSalesOrderFromPurchaseOrderEntity(this SalesOrder salesOrder, PurchaseOrder purchaseOrder, Guid modifiedBy)
        {
            salesOrder.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.EnsureUtc();
            salesOrder.ActualDeliveryDate = purchaseOrder.ActualDeliveryDate.EnsureUtc() ?? salesOrder.ActualDeliveryDate;
            salesOrder.SubTotal = purchaseOrder.SubTotal;
            salesOrder.TaxAmount = purchaseOrder.TaxAmount;
            salesOrder.DiscountAmount = purchaseOrder.DiscountAmount;
            salesOrder.TotalAmount = purchaseOrder.TotalAmount;
            salesOrder.PaymentTerms = purchaseOrder.PaymentTerms;
            salesOrder.DeliveryTerms = purchaseOrder.DeliveryTerms;

            if (salesOrder.DistributorId != purchaseOrder.DistributorId)
            {
                salesOrder.DistributorId = purchaseOrder.DistributorId;
            }
            // Append remarks instead of overwriting
            if (!string.IsNullOrWhiteSpace(purchaseOrder.Remarks))
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
                var newNoteEntry = $"[{timestamp}] {purchaseOrder.Remarks}";

                if (string.IsNullOrWhiteSpace(salesOrder.Remarks))
                {
                    salesOrder.Remarks = newNoteEntry;
                }
                else
                {
                    salesOrder.Remarks = $"{salesOrder.Remarks}\n{newNoteEntry}";
                }
            }

            salesOrder.InternalNotes = $"Sales order updated from purchase order {purchaseOrder.PONumber} on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC";
            salesOrder.ModifiedBy = modifiedBy;
            salesOrder.ModifiedOn = DateTime.UtcNow;
        }

        // Helper method to generate SO number
        private static string GenerateSONumber()
        {
            return $"SO-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }
    }
}