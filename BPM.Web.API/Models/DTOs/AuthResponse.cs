namespace BPM.Web.API.Models.DTOs
{
    public class AuthResponse
    {
        public string Message { get; set; }
        public bool IsValidUser { get; set; }
        public bool IsValidPassword { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public AuthenticateResponseDto authenticateResponseDto { get; set; }
    }
}
