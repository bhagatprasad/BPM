using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Distributor.UI.Models
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public bool ConfirmReset { get; set; }
    }
}