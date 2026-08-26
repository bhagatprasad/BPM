using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.Orders.API.Models.Entities
{
    [Table("purchase_order_approvals")]
    public class PurchaseOrderApproval
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("purchaseorderid")]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        [Column("approverid")]
        public Guid ApproverId { get; set; }

        [Required]
        [Column("approvallevel")]
        public int ApprovalLevel { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending";

        [Column("comments")]
        [MaxLength(500)]
        public string? Comments { get; set; }

        [Column("actiondate")]
        public DateTime? ActionDate { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

    }
}
