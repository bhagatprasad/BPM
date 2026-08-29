using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.InventoryManagement.API.Models.Entities
{
    [Table("stock_movements")]
    public class StockMovement
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("inventoryid")]
        public Guid InventoryId { get; set; }

        [Required]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Required]
        [Column("packagingid")]
        public Guid PackagingId { get; set; }

        [Required]
        [Column("batchid")]
        public Guid BatchId { get; set; }

        [Required]
        [Column("warehouseid")]
        public Guid WarehouseId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("movementtype")]
        public string MovementType { get; set; } = string.Empty;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("quantitybefore")]
        public int QuantityBefore { get; set; }

        [Required]
        [Column("quantityafter")]
        public int QuantityAfter { get; set; }

        [MaxLength(50)]
        [Column("referencetype")]
        public string? ReferenceType { get; set; }

        [Column("referenceid")]
        public Guid? ReferenceId { get; set; }

        [Column("unitcost", TypeName = "numeric(18,2)")]
        public decimal? UnitCost { get; set; }

        [MaxLength(500)]
        [Column("remarks")]
        public string? Remarks { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Required]
        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("distributorid")]
        public Guid? DistributorId { get; set; }
    }
}
