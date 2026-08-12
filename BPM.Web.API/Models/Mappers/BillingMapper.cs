using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.DTOs.Billing;
using BPM.Web.API.Models.DTOs;
namespace BPM.Web.API.Models.Mappers
{
   public static class BillingMapper
    {
        //createbillingdto->billing entity
        public static Billing ToEntity(this CreateBillingDto dto,SalesOrderDto salesOrder,Guid createdBy) 
        {
            var totalAmount = salesOrder.SubTotal - salesOrder.DiscountAmount + salesOrder.TaxAmount + dto.AdjustmentAmount;
            return new Billing
            {
                Id = Guid.NewGuid(),
                BillingNumber = string.Empty,//generated in service
                PurchaseOrderId=salesOrder.PurchaseOrderId,
                SalesOrderId=salesOrder.Id,
                DealerId=salesOrder.DealerId,
                BillingDate=DateTime.UtcNow,
                SubTotal=salesOrder.SubTotal,
                DiscountAmount=salesOrder.DiscountAmount,
                TaxAmount=salesOrder.TaxAmount,
                AdjustmentAmount=dto.AdjustmentAmount,
                TotalAmount=totalAmount,
                PaidAmount=0,
                PendingAmount=totalAmount,
                Status="Pending",
                CurrencyCode=salesOrder.CurrencyCode,
                PaymentTerms=salesOrder.PaymentTerms,
                Remarks=dto.Remarks,
                IsActive=true,
                CreatedBy=createdBy,
                CreatedOn=DateTime.UtcNow
            };
        }

        // Billing Entity → BillingResponseDto
        public static BillingResponseDto ToDto(this Billing entity) 
        {
            return new BillingResponseDto
            {
                Id = entity.Id,
                BillingNumber=entity.BillingNumber,
                PurchaseOrderId=entity.PurchaseOrderId,
                SalesOrderId=entity.SalesOrderId,
                DealerId=entity.DealerId,
                BillingDate=entity.BillingDate,
                SubTotal=entity.SubTotal,
                DiscountAmount= entity.DiscountAmount,
                TaxAmount=entity.TaxAmount,
                AdjustmentAmount=entity.AdjustmentAmount,
                TotalAmount=entity.TotalAmount,
                PaidAmount=entity.PaidAmount,
                PendingAmount=entity.PendingAmount,
                Status=entity.Status,
                CurrencyCode=entity.CurrencyCode,
                PaymentTerms=entity.PaymentTerms,
                Remarks=entity.Remarks,
                IsActive=entity.IsActive,
                CreatedBy=entity.CreatedBy,
                CreatedOn= entity.CreatedOn,
                ModifiedBy=entity.ModifiedBy,
                ModifiedOn= entity.ModifiedOn
            };
        }

    }
}
