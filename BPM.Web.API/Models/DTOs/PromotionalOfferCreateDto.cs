using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.Discount
{
    public class PromotionalOfferCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string OfferName { get; set; } = string.Empty;

        [Required]
        public Guid SupplierId { get; set; }

        public Guid? DrugId { get; set; }

        public Guid? PackagingId { get; set; }

        [Range(0, 50)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool AllowCombination { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
