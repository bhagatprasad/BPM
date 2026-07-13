using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class AuthenticateUserDto
    {
        public string Username { get; set; } // Email
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
