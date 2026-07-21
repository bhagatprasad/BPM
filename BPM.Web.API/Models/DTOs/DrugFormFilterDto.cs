namespace BPM.Web.API.Models.DTOs
{
    public class DrugFormFilterDto
    {
        public string? FormCode { get; set; }
        public string? FormName { get; set; }
        public string? FormType { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasDrugs { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
