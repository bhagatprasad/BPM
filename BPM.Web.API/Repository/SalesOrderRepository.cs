using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SalesOrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllSalesOrderAsync()
        {
            return await _dbContext.SalesOrders.ToListAsync();
        }

        public async Task<IEnumerable<SalesOrder>> GetSalesOrderByDealer(Guid dealerId)
        {
            return await _dbContext.SalesOrders.Where(a => a.DealerId == dealerId).ToListAsync();
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderWithItemsAsync(Guid purchaseOrderId)
        {
            return await _dbContext.PurchaseOrders.Include(a => a.PurchaseOrderItems).FirstOrDefaultAsync(a => a.Id == purchaseOrderId);
        }

        public async Task<SalesOrder> CreateSalesOrderAsync(SalesOrder salesOrder, List<SalesOrderItem> salesOrderItems)
        {
            await _dbContext.SalesOrders.AddAsync(salesOrder);
            await _dbContext.SalesOrderItems.AddRangeAsync(salesOrderItems);
            await _dbContext.SaveChangesAsync();
            return salesOrder;
        }

    }
}
