namespace BPM.Web.Identity.API.Models.DTOs
{
    public class PermissionActivityDto
    {
        public Guid PermissionId { get; set; }

        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityCode { get; set; }

        public bool IsEnabled { get; set; }
    }
}
