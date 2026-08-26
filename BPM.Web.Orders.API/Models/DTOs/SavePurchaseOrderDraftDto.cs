namespace BPM.Web.Orders.API.Models.DTOs
{
    public class SavePurchaseOrderDraftDto
    {
        public Guid? PurchaseOrderId { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? DealerId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? PaymentTerms { get; set; }
        public string? DeliveryTerms { get; set; }
        public string? Remarks { get; set; }
        public string? InternalNotes { get; set; }
        public Guid? CurrentUserId { get; set; }
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }
}
