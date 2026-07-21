namespace BPM.Web.API.Models.DTOs
{
    public class DrugPackagingFilterDto
    {
        public Guid? DrugId { get; set; }
        public Guid? PackageUomId { get; set; }
        public Guid? ContainsUomId { get; set; }
        public string? Barcode { get; set; }
        public bool? IsActive { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
