using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Extensions;
using Microsoft.IdentityModel.Tokens;

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
                ExpectedDeliveryDate = dto.ExpectedDeliveryDate.EnsureUtc(),
                PaymentTerms = dto.PaymentTerms,
                DeliveryTerms = dto.DeliveryTerms,
                Remarks = dto.Remarks,
                InternalNotes = dto.InternalNotes,
                PONumber = string.Empty, // Generated in Service
                OrderDate = DateTime.UtcNow, // Already UTC
                Status = !string.IsNullOrEmpty(dto.Status) ? dto.Status : "Draft",
                SubTotal = 0,
                TaxAmount = 0,
                DiscountAmount = 0,
                TotalAmount = 0,
                CurrencyCode = "INR",
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = dto.CreatedBy,
                ModifiedOn = DateTime.UtcNow
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
                ExpiryDate = dto.ExpiryDate.EnsureUtc(),
                Remarks = dto.Remarks,
                CreatedOn = DateTime.UtcNow // Already UTC
            };
        }

        public static PurchaseOrderResponseDto ToDto(this PurchaseOrder purchaseOrder)
        {
            return new PurchaseOrderResponseDto
            {
                Id = purchaseOrder.Id,
                PONumber = purchaseOrder.PONumber,
                SupplierId = purchaseOrder.SupplierId,
                SupplierName = purchaseOrder.Supplier?.SupplierName ?? string.Empty,
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
                ModifiedBy = purchaseOrder.ModifiedBy,
                ModifiedOn = purchaseOrder.ModifiedOn,
                Dealer = purchaseOrder.Dealer?.ToDto(),
                PurchaseOrderItemResponse = purchaseOrder.PurchaseOrderItems?
                    .Select(x => x.ToDto())
                    .ToList() ?? new List<PurchaseOrderItemResponseDto>()
            };
        }

        // Update existing entity instead of creating new
        public static void UpdateFromProcessDto(this PurchaseOrder purchaseOrder, ProcessPurchaseOrderDto dto, Guid currentUserId)
        {
            // Append new notes to existing remarks with timestamp
            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
                var newNoteEntry = $"[{timestamp}] {dto.Notes}";

                if (string.IsNullOrWhiteSpace(purchaseOrder.Remarks))
                {
                    purchaseOrder.Remarks = newNoteEntry;
                }
                else
                {
                    purchaseOrder.Remarks = $"{purchaseOrder.Remarks}\n{newNoteEntry}";
                }
            }

            purchaseOrder.Status = dto.Status;
            purchaseOrder.ModifiedBy = currentUserId;
            purchaseOrder.ModifiedOn = DateTime.UtcNow; // Already UTC

            // If status is Approved or Verified, set ActualDeliveryDate if not set
            if (string.Equals(dto.Status, "Approved", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(dto.Status, "Verified", StringComparison.OrdinalIgnoreCase))
            {
                if (purchaseOrder.ActualDeliveryDate == null || purchaseOrder.ActualDeliveryDate == DateTime.MinValue)
                {
                    purchaseOrder.ActualDeliveryDate = DateTime.UtcNow; // Already UTC
                }
            }

            // Ensure all DateTime fields are UTC
            purchaseOrder.OrderDate = purchaseOrder.OrderDate.ToDatabaseUtc();
            purchaseOrder.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.ToDatabaseUtc();
            purchaseOrder.ActualDeliveryDate = purchaseOrder.ActualDeliveryDate.ToDatabaseUtc();
            purchaseOrder.ModifiedOn = purchaseOrder.ModifiedOn.ToDatabaseUtc();
            purchaseOrder.CreatedOn = purchaseOrder.CreatedOn.ToDatabaseUtc();
        }

        // Alternative: Update with separate notes parameter
        public static void UpdateFromProcessDtoWithNotes(this PurchaseOrder purchaseOrder, ProcessPurchaseOrderDto dto, Guid currentUserId, string additionalNotes = null)
        {
            // Combine notes: dto.Notes + additionalNotes
            var combinedNotes = string.Empty;

            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                combinedNotes = dto.Notes;
            }

            if (!string.IsNullOrWhiteSpace(additionalNotes))
            {
                if (!string.IsNullOrWhiteSpace(combinedNotes))
                {
                    combinedNotes = $"{combinedNotes}\n{additionalNotes}";
                }
                else
                {
                    combinedNotes = additionalNotes;
                }
            }

            // Append combined notes to existing remarks with timestamp
            if (!string.IsNullOrWhiteSpace(combinedNotes))
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
                var newNoteEntry = $"[{timestamp}] {combinedNotes}";

                if (string.IsNullOrWhiteSpace(purchaseOrder.Remarks))
                {
                    purchaseOrder.Remarks = newNoteEntry;
                }
                else
                {
                    purchaseOrder.Remarks = $"{purchaseOrder.Remarks}\n{newNoteEntry}";
                }
            }

            purchaseOrder.Status = dto.Status;
            purchaseOrder.ModifiedBy = currentUserId;
            purchaseOrder.ModifiedOn = DateTime.UtcNow;

            // If status is Approved or Verified, set ActualDeliveryDate if not set
            if (string.Equals(dto.Status, "Approved", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(dto.Status, "Verified", StringComparison.OrdinalIgnoreCase))
            {
                if (purchaseOrder.ActualDeliveryDate == null || purchaseOrder.ActualDeliveryDate == DateTime.MinValue)
                {
                    purchaseOrder.ActualDeliveryDate = DateTime.UtcNow;
                }
            }

            // Ensure all DateTime fields are UTC
            purchaseOrder.OrderDate = purchaseOrder.OrderDate.ToDatabaseUtc();
            purchaseOrder.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate.ToDatabaseUtc();
            purchaseOrder.ActualDeliveryDate = purchaseOrder.ActualDeliveryDate.ToDatabaseUtc();
            purchaseOrder.ModifiedOn = purchaseOrder.ModifiedOn.ToDatabaseUtc();
            purchaseOrder.CreatedOn = purchaseOrder.CreatedOn.ToDatabaseUtc();
        }

        public static PurchaseOrderItemResponseDto ToDto(this PurchaseOrderItem item)
        {
            return new PurchaseOrderItemResponseDto
            {
                Id = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                DrugId = item.DrugId,
                DrugName = item.Drug?.DrugName ?? string.Empty,
                DrugCode = item.Drug?.DrugCode ?? string.Empty,
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