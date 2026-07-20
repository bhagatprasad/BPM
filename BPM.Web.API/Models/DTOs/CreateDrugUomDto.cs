using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.DrugUom
{
    public class CreateDrugUomDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        [StringLength(20)]
        public string UomCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string UomName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string UomType { get; set; } = string.Empty;

        public decimal? ConversionFactor { get; set; }

        public bool IsBaseUnit { get; set; }
    }
}