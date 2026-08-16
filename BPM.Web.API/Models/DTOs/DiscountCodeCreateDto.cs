using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.Discount
{
    public class DiscountCodeCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string DiscountCode { get; set; } = string.Empty;

        [Range(0, 50)]
        public decimal DiscountPercentage { get; set; }

        public Guid? SupplierId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool RequiresApproval { get; set; } = true;

        public bool IsApproved { get; set; } = false;

        public bool AllowCombination { get; set; } = false;

        public Guid? CreatedBy { get; set; }
    }
}