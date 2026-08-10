using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class SalesOrderMapper 
    {
        public static SalesOrderDto ToDto(this SalesOrder dto) {
            return new SalesOrderDto
            {
                Id=dto.Id,
                SONumber = dto.SONumber,
                PurchaseOrderId = dto.PurchaseOrderId,
                SupplierId = dto.SupplierId,
                DealerId= dto.DealerId,
                OrderDate = dto.OrderDate,
                ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
                ActualDeliveryDate = dto.ActualDeliveryDate,
                Status = dto.Status,
                SubTotal = dto.SubTotal,
                TaxAmount= dto.TaxAmount,
                DiscountAmount= dto.DiscountAmount,
                TotalAmount= dto.TotalAmount,
                CurrencyCode= dto.CurrencyCode,
                PaymentTerms= dto.PaymentTerms,
                DeliveryTerms= dto.DeliveryTerms,
                Remarks= dto.Remarks,
                InternalNotes= dto.InternalNotes,
                IsActive= dto.IsActive,
                CreatedBy= dto.CreatedBy,
                CreatedOn=dto.CreatedOn,
                ModifiedBy= dto.ModifiedBy,
                ModifiedOn=dto.ModifiedOn
            };
        }
        public static SalesOrderItem ToSalesOrderItem(this PurchaseOrderItem item,Guid salesOrderId)
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
                ExpiryDate = item.ExpiryDate,
                Remarks = item.Remarks,
                CreatedBy = item.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy=item.ModifiedBy,
                ModifiedOn= DateTime.UtcNow
            };
        }
    }
}
