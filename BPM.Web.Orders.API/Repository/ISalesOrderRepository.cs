using BPM.Web.API.Models.Entities;

namespace BPM.Web.Orders.API.Repository
{
    public interface ISalesOrderRepository
    {
        Task<IEnumerable<SalesOrder>> GetAllSalesOrderAsync();
        Task<SalesOrder?> GetSalesOrderByIdAsync(Guid id);
        Task<IEnumerable<SalesOrder>> GetSalesOrderByDealer(Guid dealerId);
        Task<IEnumerable<SalesOrder>> GetSalesOrderByPurchaseOrder(Guid purchaseOrderId);
        Task<SalesOrder> CreateSalesOrderAsync(SalesOrder salesOrder);
        Task<SalesOrder> ProcessSalesOrderAsync(Guid salesOrderId, string status);
        Task<SalesOrder> UpdateSalesOrderAsync(SalesOrder salesOrder);
    }
}
