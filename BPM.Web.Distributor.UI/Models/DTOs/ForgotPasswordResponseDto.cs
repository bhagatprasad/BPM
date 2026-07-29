namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class ForgotPasswordResponseDto
    {
        public bool Success { get; set; }

        public Guid? UserId { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}