using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrder> CreatePurchaseOrderAsync(CreatePurchaseOrderDto createPurchaseOrderDto);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersAllAsync();
        Task<PurchaseOrderResponseDto> GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersAllAsync();
        Task<PurchaseOrderResponseDto> GetPurchaseOrderByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByDealerAsync(Guid dealerId);

    }
}
