using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrderResponseDto>> GetAllPurchaseOrdersAsync();
        Task<List<PurchaseOrderResponseDto>> GetPurchaseOrdersByDealerAsync(Guid dealerId);
    }
}
