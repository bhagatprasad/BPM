namespace BPM.Web.API.Models.DTOs
{
    public class CreateDrugCategoryDto
    {
        public string CategoryCode { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
