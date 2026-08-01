namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class ResetPasswordDto
    {
        public Guid UserId { get; set; }

        public string NewPassword { get; set; }
    }
}