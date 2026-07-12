namespace BPM.Web.API.Models.DTOs
{
    public class UserRoleUpdateDto
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public Guid ModifiedBy { get; set; }
    }
}
