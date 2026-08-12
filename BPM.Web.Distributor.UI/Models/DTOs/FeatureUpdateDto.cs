namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class FeatureUpdateDto
    {
        public string FeatureName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}