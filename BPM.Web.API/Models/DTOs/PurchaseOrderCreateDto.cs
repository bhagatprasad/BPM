using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class PurchaseOrderCreateDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        public Guid DealerId { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Required]
        [StringLength(100)]
        public string PaymentTerms { get; set; } = string.Empty;

        [StringLength(100)]
        public string? DeliveryTerms { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [StringLength(500)]
        public string? InternalNotes { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }
    }
}
