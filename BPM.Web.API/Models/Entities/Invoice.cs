using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("invoices")]
    public class Invoice
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("invoicenumber")]
        [MaxLength(30)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        [Column("billingid")]
        public Guid BillingId { get; set; }

        [Required]
        [Column("purchaseorderid")]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        [Column("salesorderid")]
        public Guid SalesOrderId { get; set; }

        [Required]
        [Column("dealerid")]
        public Guid DealerId { get; set; }

        [Column("invoicedate")]
        public DateTime InvoiceDate { get; set; }

        [Column("subtotal", TypeName = "numeric(18,2)")]
        public decimal SubTotal { get; set; }

        [Column("discountamount", TypeName = "numeric(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column("taxamount", TypeName = "numeric(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column("adjustmentamount", TypeName = "numeric(18,2)")]
        public decimal AdjustmentAmount { get; set; }

        [Column("totalamount", TypeName = "numeric(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column("paidamount", TypeName = "numeric(18,2)")]
        public decimal PaidAmount { get; set; }

        [Column("pendingamount", TypeName = "numeric(18,2)")]
        public decimal PendingAmount { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending";

        [Required]
        [Column("currencycode")]
        [MaxLength(3)]
        public string CurrencyCode { get; set; } = "INR";

        [Required]
        [Column("paymentterms")]
        [MaxLength(100)]
        public string PaymentTerms { get; set; } = string.Empty;

        [Column("remarks")]
        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(BillingId))]
        public virtual Billing? Billing { get; set; }

        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        [ForeignKey(nameof(SalesOrderId))]
        public virtual SalesOrder? SalesOrder { get; set; }
    }
}
