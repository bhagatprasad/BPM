
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.Orders.API.Models.Entities
{
    [Table("sales_orders")]
    public class SalesOrder
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("sonumber")]
        [MaxLength(20)]
        public string SONumber { get; set; } = string.Empty;

        [Required]
        [Column("purchaseorderid")]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        [Column("supplierid")]
        public Guid SupplierId { get; set; }

        [Required]
        [Column("dealerid")]
        public Guid DealerId { get; set; }

        [Required]
        [Column("distributorid")]
        public Guid DistributorId { get; set; }
        [Column("orderdate")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column("expecteddeliverydate")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Column("actualdeliverydate")]
        public DateTime? ActualDeliveryDate { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(30)]
        public string Status { get; set; } = "Created";

        [Column("subtotal", TypeName = "numeric(18,2)")]
        public decimal SubTotal { get; set; }

        [Column("taxamount", TypeName = "numeric(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column("discountamount", TypeName = "numeric(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column("totalamount", TypeName = "numeric(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column("currencycode")]
        [MaxLength(3)]
        public string CurrencyCode { get; set; } = "INR";

        [Required]
        [Column("paymentterms")]
        [MaxLength(100)]
        public string PaymentTerms { get; set; } = string.Empty;

        [Column("deliveryterms")]
        [MaxLength(100)]
        public string? DeliveryTerms { get; set; }

        [Column("remarks")]
        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Column("internalnotes")]
        [MaxLength(500)]
        public string? InternalNotes { get; set; }

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

        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();
    }
}
