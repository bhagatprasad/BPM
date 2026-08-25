namespace BPM.Web.Identity.API.Models.DTOs
{
    public class PermissionFeatureDto
    {
        public Guid FeatureId { get; set; }

        public string FeatureName { get; set; }

        public string FeatureCode { get; set; }

        public List<PermissionActivityDto> Activities { get; set; } = new();
    }
}
