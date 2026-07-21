using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class CreateDrugFormDto
    {
        [Required]
        [MaxLength(20)]
        public string FormCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FormName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FormType { get; set; }
    }
}
