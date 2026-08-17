using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> purchaseOrderItems);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAllAsync();
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDealerAsync(Guid dealerId);
        Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task<ProductAvailabilityResponseDto> ValidateProductAvailabilityAsync(Guid drugId, Guid packagingId, int quantity);
        Task<PurchaseOrder> SubmitPurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task<PurchaseOrder> SavePurchaseOrderDraftAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> purchaseOrderItems);
        Task<IEnumerable<PurchaseOrder>> GetDraftPurchaseOrdersAsync(Guid dealerId);
        Task<bool> DeletePurchaseOrderDraftAsync(Guid purchaseOrderId);
        Task<int> GetActiveDraftCountAsync(Guid dealerId);
        Task<int> DeleteExpiredDraftPurchaseOrdersAsync();
    }
}