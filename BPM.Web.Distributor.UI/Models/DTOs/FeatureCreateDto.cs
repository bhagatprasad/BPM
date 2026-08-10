namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class FeatureCreateDto
    {
        public string FeatureName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}