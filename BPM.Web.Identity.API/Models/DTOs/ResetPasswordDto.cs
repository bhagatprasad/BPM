namespace BPM.Web.Identity.API.Models.DTOs
{
    public class ResetPasswordDto
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
