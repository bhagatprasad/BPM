using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class CreatePurchaseOrderItemDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal TaxRate { get; set; }

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? Remarks { get; set; }
    }
}
