using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Distributor.UI.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email or Username is required")]
        [Display(Name = "Email Address")]
        public string Username { get; set; } = string.Empty;
    }
}