using System.ComponentModel.DataAnnotations;

namespace BPM.Web.InventoryManagement.API.Models.DTOs
{
    public class InventoryUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        public Guid BatchId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        public Guid? DistributorId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int ReservedQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }

        public bool IsActive { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}
