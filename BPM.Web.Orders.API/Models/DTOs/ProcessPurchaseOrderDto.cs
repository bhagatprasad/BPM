namespace BPM.Web.Orders.API.Models.DTOs
{
    public class ProcessPurchaseOrderDto
    {
        public Guid PurchaseOrderId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
