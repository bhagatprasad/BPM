using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class UpdateDrugFormDto
    {
        [Required]
        public Guid FormId { get; set; }

        [Required]
        [MaxLength(20)]
        public string FormCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FormName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FormType { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
