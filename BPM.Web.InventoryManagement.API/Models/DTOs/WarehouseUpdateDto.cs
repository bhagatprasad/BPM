using System.ComponentModel.DataAnnotations;

namespace BPM.Web.InventoryManagement.API.Models.DTOs
{
    public class WarehouseUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string WarehouseName { get; set; } = string.Empty;

        public Guid? DistributorId { get; set; }

        [MaxLength(255)]
        public string? AddressLine1 { get; set; }

        [MaxLength(255)]
        public string? AddressLine2 { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        public bool IsActive { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}
