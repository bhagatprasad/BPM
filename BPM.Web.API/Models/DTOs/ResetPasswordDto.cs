namespace BPM.Web.API.Models.DTOs
{
    public class ResetPasswordDto
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
