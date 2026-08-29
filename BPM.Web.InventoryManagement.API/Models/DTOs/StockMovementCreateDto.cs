using System.ComponentModel.DataAnnotations;

namespace BPM.Web.InventoryManagement.API.Models.DTOs
{
    public class StockMovementCreateDto
    {
        [Required]
        public Guid InventoryId { get; set; }

        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        public Guid BatchId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        [Required]
        public Guid DistributorId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MovementType { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public int QuantityAfter { get; set; }

        [MaxLength(50)]
        public string? ReferenceType { get; set; }

        public Guid? ReferenceId { get; set; }

        public decimal? UnitCost { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
}
