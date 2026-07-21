namespace BPM.Web.API.Models.DTOs
{
    public class DrugPackagingDto
    {
        public Guid PackagingId { get; set; }
        public Guid DrugId { get; set; }
        public Guid PackageUomId { get; set; }
        public Guid ContainsUomId { get; set; }
        public int Quantity { get; set; }
        public int TotalUnits { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal PackagePrice { get; set; }
        public string? Barcode { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        // Navigation properties for display
        public string? DrugName { get; set; }
        public string? DrugCode { get; set; }
        public string? PackageUomName { get; set; }
        public string? PackageUomCode { get; set; }
        public string? ContainsUomName { get; set; }
        public string? ContainsUomCode { get; set; }
        public string? DisplayName => $"{DrugName} - {PackageUomName} ({Quantity} x {ContainsUomName})";
    }
}
