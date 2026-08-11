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

        public async Task<SalesOrder> CreateSalesOrderAsync(SalesOrder salesOrder)
        {
            // Generate ID for SalesOrder if not set
            if (salesOrder.Id == Guid.Empty)
            {
                salesOrder.Id = Guid.NewGuid();
            }

            // Generate IDs for all items if not set
            if (salesOrder.SalesOrderItems != null && salesOrder.SalesOrderItems.Any())
            {
                foreach (var item in salesOrder.SalesOrderItems)
                {
                    // If ID is empty, generate a new one
                    if (item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid();
                    }

                    // Set the foreign key
                    item.SalesOrderId = salesOrder.Id;
                }
            }

            await _dbContext.SalesOrders.AddAsync(salesOrder);
            await _dbContext.SaveChangesAsync();

            return salesOrder;
        }

    }
}
