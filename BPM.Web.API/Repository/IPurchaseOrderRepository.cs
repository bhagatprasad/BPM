using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder,List<PurchaseOrderItem> purchaseOrderItems);
    }
}