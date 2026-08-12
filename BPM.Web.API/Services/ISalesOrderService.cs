using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync();
        Task<SalesOrderDto?> GetSalesOrderByIdAsync(Guid id);
        Task<IEnumerable<SalesOrderDto>> GetSalesOrderByDealerAsync(Guid dealerId);
        Task<SalesOrderDto> CreateSalesOrderFromPurchaseOrderAsync(Guid purchaseOrderId, Guid createdBy);
    }
}
