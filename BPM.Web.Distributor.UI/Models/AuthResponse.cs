using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Models
{
    public class AuthResponse
    {
        public string Message { get; set; }

        public bool IsValidUser { get; set; }

        public bool IsValidPassword { get; set; }

        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public AuthenticateResponseDto AuthenticateResponseDto { get; set; }
    }
}