using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder,List<PurchaseOrderItem> purchaseOrderItems);
        Task<IEnumerable<PurchaseOrder>>GetPurchaseOrdersAllAsync();
        Task<PurchaseOrder?>GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDealerAsync(Guid dealerId);


    }
}