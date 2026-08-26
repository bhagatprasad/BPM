using BPM.Web.Orders.API.Models.DTOs;

namespace BPM.Web.Orders.API.Services
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync();
        Task<SalesOrderDto?> GetSalesOrderByIdAsync(Guid id);
        Task<IEnumerable<SalesOrderDto>> GetSalesOrderByDealerAsync(Guid dealerId);
        Task<SalesOrderDto> CreateSalesOrderFromPurchaseOrderAsync(Guid purchaseOrderId, Guid createdBy);
        Task<SalesOrderDto> ProcessSalesOrderAsync(ProcessSalesOrderDto processSalesOrderDto, Guid currentUserId);
    }
}
