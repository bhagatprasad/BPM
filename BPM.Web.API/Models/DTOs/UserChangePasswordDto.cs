namespace BPM.Web.API.Models.DTOs
{
    public class UserChangePasswordDto
    {
        public Guid UserId { get; set; }

        public string OldPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public Guid ModifiedBy { get; set; }
    }
}
