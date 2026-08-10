using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface ISalesOrderRepository
    {
        Task<IEnumerable<SalesOrder>> GetAllSalesOrderAsync();
        Task<IEnumerable<SalesOrder>> GetSalesOrderByDealer(Guid dealerId);
        Task<PurchaseOrder?> GetPurchaseOrderWithItemsAsync(Guid purchaseOrderId);
        Task<SalesOrder> CreateSalesOrderAsync(SalesOrder salesOrder, List<SalesOrderItem> salesOrderItems);
    }
}
