using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class CreateDrugUomDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UomCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string UomName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string UomType { get; set; } = string.Empty;

        public Guid? ParentUomId { get; set; }

        [Range(1, int.MaxValue)]
        public int? QuantityPerParent { get; set; }

        [Required]
        [Range(0.0001, 9999999999.9999)]
        public decimal ConversionFactor { get; set; } = 1;

        public bool IsBaseUnit { get; set; }

        public bool IsPurchaseUom { get; set; }

        public bool IsSalesUom { get; set; } = true;

        public bool IsInventoryUom { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int DisplayOrder { get; set; } = 1;

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}