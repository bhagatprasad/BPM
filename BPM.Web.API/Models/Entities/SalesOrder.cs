using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("sales_orders")]
    public class SalesOrder
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("sonumber")]
        public string SONumber { get; set; } = string.Empty;

        [Column("purchaseorderid")]
        public Guid PurchaseOrderId { get; set; }

        [Column("supplierid")]
        public Guid SupplierId { get; set; }

        [Column("dealerid")]
        public Guid DealerId { get; set; }

        [Column("orderdate")]
        public DateTime OrderDate { get; set; }

        [Column("expecteddeliverydate")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Column("actualdeliverydate")]
        public DateTime? ActualDeliveryDate { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Created";

        [Column("subtotal")]
        public decimal SubTotal { get; set; }

        [Column("taxamount")]
        public decimal TaxAmount { get; set; }

        [Column("discountamount")]
        public decimal DiscountAmount { get; set; }

        [Column("totalamount")]
        public decimal TotalAmount { get; set; }

        [Column("currencycode")]
        public string CurrencyCode { get; set; } = "INR";

        [Column("paymentterms")]
        public string PaymentTerms { get; set; }=string.Empty;

        [Column("deliveryterms")]
        public string? DeliveryTerms { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }

        [Column("internalnotes")]
        public string? InternalNotes { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
       
        // Navigation Properties

        public virtual List<SalesOrderItem> SalesOrderItems { get; set; } = new();

    }
}
