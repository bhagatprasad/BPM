using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class UpdateDrugPackagingDto
    {
        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PackageUomId { get; set; }

        [Required]
        public Guid ContainsUomId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalUnits { get; set; }

        [Required]
        [Range(0.01, 9999999999.99)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0.01, 9999999999.99)]
        public decimal PackagePrice { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Range(0, 99999999.99)]
        public decimal? GrossWeight { get; set; }

        [Range(0, 99999999.99)]
        public decimal? NetWeight { get; set; }

        [Range(0, 99999999.99)]
        public decimal? Length { get; set; }

        [Range(0, 99999999.99)]
        public decimal? Width { get; set; }

        [Range(0, 99999999.99)]
        public decimal? Height { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
