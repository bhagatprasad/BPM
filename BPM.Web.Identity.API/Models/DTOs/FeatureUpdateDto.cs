namespace BPM.Web.Identity.API.Models.DTOs
{
    public class FeatureUpdateDto
    {
        public string FeatureName { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}
