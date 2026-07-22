using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        public Guid DealerId { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Required]
        public string PaymentTerms { get; set; } = string.Empty;

        public string? DeliveryTerms { get; set; }

        public string? Remarks { get; set; }

        public string? InternalNotes { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }

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