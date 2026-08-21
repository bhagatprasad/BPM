using BPM.Web.API.Models.DTOs.PurchaseOrder;

namespace BPM.Web.API.Models.DTOs
{
    public class SalesOrderDto
    {
        public Guid Id { get; set; }

        public string SONumber { get; set; } = string.Empty;

        public Guid PurchaseOrderId { get; set; }

        public Guid SupplierId { get; set; }

        public Guid DealerId { get; set; }

        public Guid DistributorId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string CurrencyCode { get; set; } = "INR";

        public string PaymentTerms { get; set; } = string.Empty;

        public string? DeliveryTerms { get; set; }

        public string? Remarks { get; set; }

        public string? InternalNotes { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Navigation / Response DTOs

        public PurchaseOrderResponseDto? PurchaseOrder { get; set; }

        public List<SalesOrderItemDto> SalesOrderItems { get; set; }
            = new List<SalesOrderItemDto>();
    }
}