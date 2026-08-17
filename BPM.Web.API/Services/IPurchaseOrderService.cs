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
        Task<ProductAvailabilityResponseDto> ValidateProductAvailabilityAsync(Guid drugId, Guid packagingId, int quantity);
        Task<PurchaseOrderResponseDto> SubmitPurchaseOrderAsync(SubmitPurchaseOrderDto dto,Guid currentUserId);
        Task<PurchaseOrderResponseDto> SavePurchaseOrderDraftAsync(SavePurchaseOrderDraftDto dto, Guid currentUserId);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetDraftPurchaseOrdersAsync(Guid dealerId);
        Task<bool> DeletePurchaseOrderDraftAsync(Guid purchaseOrderId, Guid currentUserId);
        Task<int> DeleteExpiredDraftPurchaseOrdersAsync();


    }
}
