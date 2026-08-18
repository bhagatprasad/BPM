namespace BPM.Web.API.Models.DTOs
{
    public class PurchaseOrderApprovalActionDto
    {
        public Guid PurchaseOrderApprovalId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Comments { get; set; }
    }
}