using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class WarehouseCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string WarehouseCode { get; set; } = string.Empty;

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

        public Guid? CreatedBy { get; set; }
    }
}