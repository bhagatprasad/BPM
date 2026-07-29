using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;
    }
}