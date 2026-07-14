using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrderCreateDto purchaseOrderCreateDto);
    }
}
