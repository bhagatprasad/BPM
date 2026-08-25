namespace BPM.Web.Identity.API.Models.DTOs
{
    public class PermissionUpdateDto
    {
        public Guid RoleId { get; set; }

        public Guid FeatureId { get; set; }

        public Guid ActivityId { get; set; }

        public bool IsEnabled { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}
