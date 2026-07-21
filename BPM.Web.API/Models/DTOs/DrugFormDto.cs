namespace BPM.Web.API.Models.DTOs
{
    public class DrugFormDto
    {
        public Guid FormId { get; set; }
        public string FormCode { get; set; } = string.Empty;
        public string FormName { get; set; } = string.Empty;
        public string? FormType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int DrugCount { get; set; }
        public string DisplayName => $"{FormCode} - {FormName}";
    }
}
