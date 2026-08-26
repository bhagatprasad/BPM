using BPM.Web.Orders.API.Models.DTOs;
using BPM.Web.Orders.API.Models.Entities;

namespace BPM.Web.Orders.API.Models.Mappers
{
    public static class PurchaseOrderApprovalMapper
    {
        public static PurchaseOrderApprovalResponseDto ToDto(this PurchaseOrderApproval entity)
        {
            return new PurchaseOrderApprovalResponseDto
            {
                Id = entity.Id,
                PurchaseOrderId = entity.PurchaseOrderId,
                ApproverId = entity.ApproverId,
                ApprovalLevel = entity.ApprovalLevel,
                Status = entity.Status,
                Comments = entity.Comments,
                ActionDate = entity.ActionDate,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }
    }
}
