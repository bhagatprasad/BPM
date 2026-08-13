using BPM.Web.API.Models.DTOs.Invoice;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class InvoiceMapper
    {
        // CreateInvoiceDto → Invoice Entity
        public static Invoice ToEntity(
            this CreateInvoiceDto dto,
            Billing billing,
            Guid createdBy)
        {
            var totalAmount =
                billing.SubTotal
                - billing.DiscountAmount
                + billing.TaxAmount
                + dto.AdjustmentAmount;

            return new Invoice
            {
                Id = Guid.NewGuid(),

                InvoiceNumber = string.Empty, // Generated in service

                BillingId = billing.Id,

                PurchaseOrderId = billing.PurchaseOrderId,

                SalesOrderId = billing.SalesOrderId,

                DealerId = billing.DealerId,

                InvoiceDate = DateTime.UtcNow,

                SubTotal = billing.SubTotal,

                DiscountAmount = billing.DiscountAmount,

                TaxAmount = billing.TaxAmount,

                AdjustmentAmount = dto.AdjustmentAmount,

                TotalAmount = totalAmount,

                PaidAmount = 0,

                PendingAmount = totalAmount,

                Status = "Pending",

                CurrencyCode = billing.CurrencyCode,

                PaymentTerms = billing.PaymentTerms,

                Remarks = dto.Remarks,

                IsActive = true,

                CreatedBy = createdBy,

                CreatedOn = DateTime.UtcNow
            };
        }

        // Invoice Entity → InvoiceResponseDto
        public static InvoiceResponseDto ToDto(this Invoice entity)
        {
            return new InvoiceResponseDto
            {
                Id = entity.Id,

                InvoiceNumber = entity.InvoiceNumber,

                BillingId = entity.BillingId,

                PurchaseOrderId = entity.PurchaseOrderId,

                SalesOrderId = entity.SalesOrderId,

                DealerId = entity.DealerId,

                InvoiceDate = entity.InvoiceDate,

                SubTotal = entity.SubTotal,

                DiscountAmount = entity.DiscountAmount,

                TaxAmount = entity.TaxAmount,

                AdjustmentAmount = entity.AdjustmentAmount,

                TotalAmount = entity.TotalAmount,

                PaidAmount = entity.PaidAmount,

                PendingAmount = entity.PendingAmount,

                Status = entity.Status,

                CurrencyCode = entity.CurrencyCode,

                PaymentTerms = entity.PaymentTerms,

                Remarks = entity.Remarks,

                IsActive = entity.IsActive,

                CreatedBy = entity.CreatedBy,

                CreatedOn = entity.CreatedOn,

                ModifiedBy = entity.ModifiedBy,

                ModifiedOn = entity.ModifiedOn
            };
        }
    }
}