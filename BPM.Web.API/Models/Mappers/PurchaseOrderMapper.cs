using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class PurchaseOrderMapper
    {
        public static PurchaseOrder ToEntity(PurchaseOrderCreateDto dto)
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

                PONumber = string.Empty,     // Will be generated in Service
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
    }
}