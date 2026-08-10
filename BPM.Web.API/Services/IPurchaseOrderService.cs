using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
namespace BPM.Web.API.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto createPurchaseOrderDto);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersAllAsync();
        Task<PurchaseOrderResponseDto> GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByDealerAsync(Guid dealerId);
        Task<PurchaseOrderResponseDto> ProcessPurchaseOrderAsync(ProcessPurchaseOrderDto processPurchaseOrderDto,Guid currentUserId);

    }
}
