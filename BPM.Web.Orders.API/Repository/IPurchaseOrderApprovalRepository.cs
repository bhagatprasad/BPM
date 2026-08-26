using BPM.Web.API.Models.Entities;

namespace BPM.Web.Orders.API.Repository
{
    public interface IPurchaseOrderApprovalRepository
    {
        Task<PurchaseOrderApproval> CreateApprovalAsync(PurchaseOrderApproval approval);
        Task<List<PurchaseOrderApproval>> CreateApprovalsAsync(List<PurchaseOrderApproval> approvals);
        Task<List<PurchaseOrderApproval>> GetApprovalsByPurchaseOrderIdAsync(Guid purchaseOrderId);
        Task<PurchaseOrderApproval?> GetApprovalByIdAsync(Guid approvalId);
        Task<PurchaseOrderApproval> UpdateApprovalAsync(PurchaseOrderApproval approval);
        Task<List<User>> GetActiveApproversAsync();
        Task<PurchaseOrder> SubmitPurchaseOrderWithApprovalsAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderApproval> approvalRecords);
    }
}
