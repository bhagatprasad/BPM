namespace BPM.Web.API.Models.DTOs.PurchaseOrder
{
    public class PurchaseOrderResponseDto
    {
        public Guid Id { get; set; }

        public string PONumber { get; set; } = string.Empty;

        public Guid SupplierId { get; set; }

        public Guid DealerId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? PaymentTerms { get; set; }

        public string? DeliveryTerms { get; set; }

        public string? Remarks { get; set; }
    }
}
