using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Distributor.UI.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}