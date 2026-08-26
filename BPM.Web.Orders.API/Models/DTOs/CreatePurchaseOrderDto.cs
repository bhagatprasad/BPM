using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Orders.API.Models.DTOs
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        public Guid DealerId { get; set; }

        [Required]
        public Guid DistributorId { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Required]
        public string PaymentTerms { get; set; } = string.Empty;

        public string? DeliveryTerms { get; set; }

        public string? Remarks { get; set; }

        public string? Status { get; set; }

        public string? InternalNotes { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }
}
