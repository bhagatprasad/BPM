namespace BPM.Web.API.Models.DTOs.Invoice
{
    public class InvoiceResponseDto
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public Guid BillingId { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Guid SalesOrderId { get; set; }

        public Guid DealerId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal AdjustmentAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal PendingAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = string.Empty;

        public string PaymentTerms { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}