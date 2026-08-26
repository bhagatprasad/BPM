using BPM.Web.API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.Orders.API.Models.Entities
{
    [Table("sales_order_items")]
    public class SalesOrderItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("salesorderid")]
        public Guid SalesOrderId { get; set; }

        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unitprice")]
        public decimal UnitPrice { get; set; }

        [Column("discountpercentage")]
        public decimal DiscountPercentage { get; set; }

        [Column("discountamount")]
        public decimal DiscountAmount { get; set; }

        [Column("taxrate")]
        public decimal TaxRate { get; set; }

        [Column("taxamount")]
        public decimal TaxAmount { get; set; }

        [Column("totalamount")]
        public decimal TotalAmount { get; set; }

        [Column("receivedquantity")]
        public int ReceivedQuantity { get; set; }

        [Column("pendingquantity")]
        public int PendingQuantity { get; set; }

        [Column("batchnumber")]
        public string? BatchNumber { get; set; }

        [Column("expirydate")]
        public DateTime? ExpiryDate { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        [Column("packagingid")]
        public Guid PackagingId { get; set; }

        // Navigation Property

        [ForeignKey(nameof(SalesOrderId))]
        public virtual SalesOrder? SalesOrder { get; set; }

        [ForeignKey(nameof(DrugId))]
        public virtual Drug? Drug { get; set; }
    }
}
}
