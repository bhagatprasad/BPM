using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("purchaseorderitems")]
    public class PurchaseOrderItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("purchaseorderid")]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("unitprice", TypeName = "numeric(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column("discountpercentage", TypeName = "numeric(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column("discountamount", TypeName = "numeric(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column("taxrate", TypeName = "numeric(5,2)")]
        public decimal TaxRate { get; set; }

        [Column("taxamount", TypeName = "numeric(18,2)")]
        public decimal TaxAmount { get; set; }

        [Required]
        [Column("totalamount", TypeName = "numeric(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column("receivedquantity")]
        public int ReceivedQuantity { get; set; }

        [Column("pendingquantity")]
        public int PendingQuantity { get; set; }

        [Column("batchnumber")]
        [MaxLength(100)]
        public string? BatchNumber { get; set; }

        [Column("expirydate")]
        public DateTime? ExpiryDate { get; set; }

        [Column("remarks")]
        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
