namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class UserChangePasswordDto
    {
        public Guid UserId { get; set; }

        public string NewPassword { get; set; } = string.Empty;

        public Guid ModifiedBy { get; set; }
    }
}
