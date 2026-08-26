namespace BPM.Web.Orders.API.Models.DTOs
{
    public class PurchaseOrderApprovalResponseDto
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Guid ApproverId { get; set; }

        public int ApprovalLevel { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Comments { get; set; }

        public DateTime? ActionDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
