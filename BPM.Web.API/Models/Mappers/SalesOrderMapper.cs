using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;

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
                ModifiedOn = entity.ModifiedOn
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
                ModifiedOn = entity.ModifiedOn
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
                OrderDate = DateTime.UtcNow,
                ExpectedDeliveryDate = GetUtcDateTime(purchaseOrder.ExpectedDeliveryDate, DateTime.UtcNow.AddDays(7)),
                ActualDeliveryDate = GetUtcDateTime(purchaseOrder.ActualDeliveryDate, DateTime.UtcNow.AddDays(7)),
                Status = "Created",
                SubTotal = purchaseOrder.SubTotal,
                TaxAmount = purchaseOrder.TaxAmount,
                DiscountAmount = purchaseOrder.DiscountAmount,
                TotalAmount = purchaseOrder.TotalAmount,
                CurrencyCode = "INR",
                PaymentTerms = purchaseOrder.PaymentTerms,
                DeliveryTerms = purchaseOrder.DeliveryTerms,
                Remarks = purchaseOrder.Remarks,
                InternalNotes = "",
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
                ExpiryDate = GetUtcDateTime(item.ExpiryDate, DateTime.UtcNow.AddYears(1)),
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
                Id = Guid.NewGuid(),
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
                ExpiryDate = GetUtcDateTime(item.ExpiryDate, DateTime.UtcNow.AddYears(1)),
                Remarks = item.Remarks,
                CreatedBy = null,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = null,
                ModifiedOn = null
            };
        }

        public static void UpdateSalesOrderFromPurchaseOrder(this SalesOrder salesOrder, PurchaseOrderResponseDto purchaseOrder, Guid modifiedBy)
        {
            salesOrder.ExpectedDeliveryDate = GetUtcDateTime(purchaseOrder.ExpectedDeliveryDate, salesOrder.ExpectedDeliveryDate);
            salesOrder.ActualDeliveryDate = GetUtcDateTime(purchaseOrder.ActualDeliveryDate, salesOrder.ActualDeliveryDate.Value);
            salesOrder.SubTotal = purchaseOrder.SubTotal;
            salesOrder.TaxAmount = purchaseOrder.TaxAmount;
            salesOrder.DiscountAmount = purchaseOrder.DiscountAmount;
            salesOrder.TotalAmount = purchaseOrder.TotalAmount;
            salesOrder.PaymentTerms = purchaseOrder.PaymentTerms;
            salesOrder.DeliveryTerms = purchaseOrder.DeliveryTerms;
            salesOrder.Remarks = purchaseOrder.Remarks;
            salesOrder.ModifiedBy = modifiedBy;
            salesOrder.ModifiedOn = DateTime.UtcNow;
        }

        // Helper method to generate SO number
        private static string GenerateSONumber()
        {
            return $"SO-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }

        // Helper method to get UTC DateTime with default value
        private static DateTime GetUtcDateTime(DateTime? dateTime, DateTime defaultValue)
        {
            var value = dateTime ?? defaultValue;
            return value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();
        }

        // Helper method to ensure DateTime is UTC (nullable)
        public static DateTime? EnsureUtc(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return null;

            return dateTime.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc)
                : dateTime.Value.ToUniversalTime();
        }

        // Overload for non-nullable DateTime
        public static DateTime EnsureUtc(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                : dateTime.ToUniversalTime();
        }
    }
}