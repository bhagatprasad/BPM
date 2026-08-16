using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("batch_master")]
    public class BatchMaster
    {
        [Key]
        [Column("batchid")]
        public Guid BatchId { get; set; }

        [Column("batchnumber")]
        [MaxLength(100)]
        public string? BatchNumber { get; set; }

        [Column("batch_reference")]
        [MaxLength(100)]
        public string? BatchReference { get; set; }

        [Column("supplierid")]
        public Guid? SupplierId { get; set; }

        [Column("supplier_invoice_number")]
        [MaxLength(100)]
        public string? SupplierInvoiceNumber { get; set; }

        [Column("supplier_invoice_date")]
        public DateTime? SupplierInvoiceDate { get; set; }

        [Column("received_date")]
        public DateTime? ReceivedDate { get; set; }

        [Column("manufacturing_date")]
        public DateTime? ManufacturingDate { get; set; }

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("total_quantity")]
        public int TotalQuantity { get; set; }

        [Column("total_value", TypeName = "numeric(18,2)")]
        public decimal TotalValue { get; set; }

        [Column("total_tax", TypeName = "numeric(18,2)")]
        public decimal TotalTax { get; set; }

        [Column("total_discount", TypeName = "numeric(18,2)")]
        public decimal TotalDiscount { get; set; }

        [Column("net_amount", TypeName = "numeric(18,2)")]
        public decimal NetAmount { get; set; }

        [Required]
        [Column("batch_status")]
        [MaxLength(20)]
        public string BatchStatus { get; set; } = "ACTIVE";

        [Column("warehouseid")]
        public Guid? WarehouseId { get; set; }

        [Column("storage_location")]
        [MaxLength(100)]
        public string? StorageLocation { get; set; }

        [Column("payment_terms")]
        [MaxLength(100)]
        public string? PaymentTerms { get; set; }

        [Column("delivery_terms")]
        [MaxLength(100)]
        public string? DeliveryTerms { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime? CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
    }
}
