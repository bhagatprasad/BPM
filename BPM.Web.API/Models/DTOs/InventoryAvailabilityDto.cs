using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class InventoryAvailabilityDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        public Guid BatchId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        [Range(1, int.MaxValue)]
        public int RequestedQuantity { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsAvailable { get; set; }
    }
}