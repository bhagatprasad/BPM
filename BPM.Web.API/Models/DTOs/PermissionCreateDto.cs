namespace BPM.Web.API.Models.DTOs
{
    public class PermissionCreateDto
    {
        public Guid RoleId { get; set; }

        public Guid FeatureId { get; set; }

        public Guid ActivityId { get; set; }

        public bool IsEnabled { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
