using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Distributor.UI.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
    }
}