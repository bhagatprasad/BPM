using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using log4net.Util;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchaseOrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> purchaseOrderItems)
        {
            await _dbContext.PurchaseOrders.AddAsync(purchaseOrder);

            await _dbContext.SaveChangesAsync();

            foreach (var item in purchaseOrderItems)
            {
                item.PurchaseOrderId = purchaseOrder.Id;
            }

            await _dbContext.PurchaseOrderItems.AddRangeAsync(purchaseOrderItems);
            await _dbContext.SaveChangesAsync();

            return purchaseOrder;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAllAsync()
        {
            return await _dbContext.PurchaseOrders.Where(po => po.IsActive).Include(po=>po.Supplier).Include(po => po.PurchaseOrderItems).ThenInclude(item => item.Drug).OrderByDescending(po => po.ModifiedOn).ToListAsync();
        }
    

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid id)
        {
            return await _dbContext.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                .ThenInclude(item => item.Drug)
                .FirstOrDefaultAsync(po => po.Id == id && po.IsActive);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDealerAsync(Guid dealerId)
        {
            return await _dbContext.PurchaseOrders
                .Include(po=>po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                .ThenInclude(item => item.Drug)
                .Where(po => po.DealerId == dealerId && po.IsActive)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();
        }

        public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _dbContext.PurchaseOrders.Update(purchaseOrder);
            await _dbContext.SaveChangesAsync();
            return purchaseOrder;
        }
    }
}