using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class PurchaseOrderMapper
    {
        public static PurchaseOrder ToEntity(this CreatePurchaseOrderDto dto)
        {
            return new PurchaseOrder
            {
                SupplierId = dto.SupplierId,
                DealerId = dto.DealerId,

                ExpectedDeliveryDate = dto.ExpectedDeliveryDate,

                PaymentTerms = dto.PaymentTerms,
                DeliveryTerms = dto.DeliveryTerms,
                Remarks = dto.Remarks,
                InternalNotes = dto.InternalNotes,

                PONumber = string.Empty, // Generated in Service
                OrderDate = DateTime.UtcNow,
                Status = "Draft",

                SubTotal = 0,
                TaxAmount = 0,
                DiscountAmount = 0,
                TotalAmount = 0,

                CurrencyCode = "INR",

                IsActive = true,

                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static PurchaseOrderItem ToEntity(this CreatePurchaseOrderItemDto dto)
        {
            return new PurchaseOrderItem
            {
                DrugId = dto.DrugId,
                PackagingId = dto.PackagingId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,

                DiscountPercentage = dto.DiscountPercentage,
                DiscountAmount = 0,

                TaxRate = dto.TaxRate,
                TaxAmount = 0,

                TotalAmount = 0,

                ReceivedQuantity = 0,
                PendingQuantity = dto.Quantity,

                BatchNumber = dto.BatchNumber,
                ExpiryDate = dto.ExpiryDate,
                Remarks = dto.Remarks,

                CreatedOn = DateTime.UtcNow
            };
        }
        public static PurchaseOrderResponseDto ToDto(this PurchaseOrder purchaseOrder)
        {
            return new PurchaseOrderResponseDto
            {
                Id = purchaseOrder.Id,
                PONumber = purchaseOrder.PONumber,
                SupplierId = purchaseOrder.SupplierId,
                DealerId = purchaseOrder.DealerId,
                OrderDate = purchaseOrder.OrderDate,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
                ActualDeliveryDate = purchaseOrder.ActualDeliveryDate,
                Status = purchaseOrder.Status,
                SubTotal = purchaseOrder.SubTotal,
                TaxAmount = purchaseOrder.TaxAmount,
                DiscountAmount = purchaseOrder.DiscountAmount,
                TotalAmount = purchaseOrder.TotalAmount,
                PaymentTerms = purchaseOrder.PaymentTerms,
                DeliveryTerms = purchaseOrder.DeliveryTerms,
                Remarks = purchaseOrder.Remarks,

                PurchaseOrderItemResponse = purchaseOrder.PurchaseOrderItems?
                    .Select(x => x.ToDto())
                    .ToList() ?? new List<PurchaseOrderItemResponseDto>()
            };
        }
        public static PurchaseOrderItemResponseDto ToDto(this PurchaseOrderItem item)
        {
            return new PurchaseOrderItemResponseDto
            {
                Id = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                DrugId = item.DrugId,
                PackagingId = item.PackagingId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountPercentage = item.DiscountPercentage,
                DiscountAmount = item.DiscountAmount,
                TaxRate = item.TaxRate,
                TaxAmount = item.TaxAmount,
                TotalAmount = item.TotalAmount,
                ReceivedQuantity = item.ReceivedQuantity,
                PendingQuantity = item.PendingQuantity,
                BatchNumber = item.BatchNumber,
                ExpiryDate = item.ExpiryDate,
                Remarks = item.Remarks,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn,
                ModifiedBy = item.ModifiedBy,
                ModifiedOn = item.ModifiedOn
            };
        }
    }
}