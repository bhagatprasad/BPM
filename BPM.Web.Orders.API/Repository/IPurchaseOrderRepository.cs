using BPM.Web.Orders.API.Models.DTOs;
using BPM.Web.Orders.API.Models.Entities;

namespace BPM.Web.Orders.API.Repository
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> purchaseOrderItems);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAllAsync();
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDealerAsync(Guid dealerId);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDistributorAsync(Guid distributorId);
        Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task<ProductAvailabilityResponseDto> ValidateProductAvailabilityAsync(Guid drugId, Guid packagingId, int quantity);
        Task<PurchaseOrder> SubmitPurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task<PurchaseOrder> SavePurchaseOrderDraftAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> purchaseOrderItems);
        Task<IEnumerable<PurchaseOrder>> GetDraftPurchaseOrdersAsync(Guid dealerId);
        Task<bool> DeletePurchaseOrderDraftAsync(Guid purchaseOrderId);
        Task<int> GetActiveDraftCountAsync(Guid dealerId);
        Task<int> DeleteExpiredDraftPurchaseOrdersAsync();
        Task<decimal> GetCurrentDiscountPercentageAsync(Guid supplierId, Guid drugId, Guid packagingId, int quantity);
    }
}
