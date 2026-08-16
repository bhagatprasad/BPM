namespace BPM.Web.API.Models.DTOs.StockMovement
{
    public class StockMovementResponseDto
    {
        public Guid Id { get; set; }

        public Guid InventoryId { get; set; }

        public Guid DrugId { get; set; }

        public Guid PackagingId { get; set; }

        public Guid BatchId { get; set; }

        public Guid WarehouseId { get; set; }

        public Guid? DistributorId { get; set; }

        public string MovementType { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public int QuantityBefore { get; set; }

        public int QuantityAfter { get; set; }

        public string? ReferenceType { get; set; }

        public Guid? ReferenceId { get; set; }

        public decimal? UnitCost { get; set; }

        public string? Remarks { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}