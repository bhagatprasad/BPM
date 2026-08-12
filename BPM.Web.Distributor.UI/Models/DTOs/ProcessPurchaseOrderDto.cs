namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class ProcessPurchaseOrderDto
    {
        public Guid PurchaseOrderId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
