namespace BPM.Web.API.Models.DTOs.PurchaseOrder
{
    public class ProductAvailabilityResponseDto
    {
        public Guid DrugId { get; set; }
        public Guid PackagingId { get; set; }
        public int RequestedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
