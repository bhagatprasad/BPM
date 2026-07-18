namespace BPM.Web.API.Models.DTOs
{
    public class IdentifyUserResponseDto
    {
        public bool Success { get; set; }
        public Guid? UserId { get; set; }
        public string Message { get; set; }
    }
}
