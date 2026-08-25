namespace BPM.Web.Identity.API.Models.DTOs
{
    public class PermissionDto
    {
        public Guid PermissionId { get; set; }

        public Guid RoleId { get; set; }

        public Guid FeatureId { get; set; }

        public Guid ActivityId { get; set; }

        public bool IsEnabled { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
