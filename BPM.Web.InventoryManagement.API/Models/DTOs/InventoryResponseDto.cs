namespace BPM.Web.InventoryManagement.API.Models.DTOs
{
    public class InventoryResponseDto
    {
        public Guid Id { get; set; }

        public Guid DrugId { get; set; }

        public Guid PackagingId { get; set; }

        public Guid BatchId { get; set; }

        public Guid WarehouseId { get; set; }

        public Guid? DistributorId { get; set; }

        public int Quantity { get; set; }

        public int ReservedQuantity { get; set; }

        public int AvailableQuantity { get; set; }

        public int ReorderLevel { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
