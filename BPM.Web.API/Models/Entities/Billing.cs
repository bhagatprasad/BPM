using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("billings")]
    public class Billing
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("billingnumber")]
        [MaxLength(30)]
        public string BillingNumber { get; set; } = string.Empty;

        [Required]
        [Column("purchaseorderid")]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        [Column("salesorderid")]
        public Guid SalesOrderId { get; set; }

        [Required]
        [Column("dealerid")]
        public Guid DealerId { get; set; }

        [Required]
        [Column("billingdate")]
        public DateTime BillingDate { get; set; }

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

        [Column("paymentterms")]
        [MaxLength(100)]
        public string? PaymentTerms { get; set; }

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

        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        [ForeignKey(nameof(SalesOrderId))]
        public virtual SalesOrder? SalesOrder { get; set; }

        [ForeignKey(nameof(DealerId))]
        public virtual Dealer? Dealer { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? CreatedUser { get; set; }

        [ForeignKey(nameof(ModifiedBy))]
        public virtual User? ModifiedUser { get; set; }
    }
}
