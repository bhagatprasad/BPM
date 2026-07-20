namespace BPM.Web.API.Models.DTOs
{
    public class DrugUomDto
    {
        public Guid UomId { get; set; }

        public Guid DrugId { get; set; }

        public string UomCode { get; set; } = string.Empty;

        public string UomName { get; set; } = string.Empty;

        public string UomType { get; set; } = string.Empty;

        public decimal? ConversionFactor { get; set; }

        public bool IsBaseUnit { get; set; }

        public bool IsActive { get; set; }
    }
}