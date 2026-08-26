using BPM.Web.Orders.API.Models.DTOs;

namespace BPM.Web.Orders.API.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto createPurchaseOrderDto);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersAllAsync();
        Task<PurchaseOrderResponseDto> GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByDealerAsync(Guid dealerId);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByDistributorAsync(Guid distributorId);
        Task<PurchaseOrderResponseDto> ProcessPurchaseOrderAsync(ProcessPurchaseOrderDto processPurchaseOrderDto, Guid currentUserId);
        Task<ProductAvailabilityResponseDto> ValidateProductAvailabilityAsync(Guid drugId, Guid packagingId, int quantity);
        Task<PurchaseOrderResponseDto> SubmitPurchaseOrderAsync(SubmitPurchaseOrderDto dto, Guid currentUserId);
        Task<PurchaseOrderResponseDto> SavePurchaseOrderDraftAsync(SavePurchaseOrderDraftDto dto, Guid currentUserId);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetDraftPurchaseOrdersAsync(Guid dealerId);
        Task<bool> DeletePurchaseOrderDraftAsync(Guid purchaseOrderId, Guid currentUserId);
        Task<int> DeleteExpiredDraftPurchaseOrdersAsync();
        Task<PurchaseOrderResponseDto> CopyPurchaseOrderAsync(Guid purchaseOrderId, Guid currentUserId);
    }
}
