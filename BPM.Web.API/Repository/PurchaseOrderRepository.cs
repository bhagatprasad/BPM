using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;

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
            return await _dbContext.PurchaseOrders.Where(po=>po.IsActive).OrderByDescending(po=>po.CreatedOn).ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDealerAsync(Guid dealerId)
        {
           return await _dbContext.PurchaseOrders
                .Where(po => po.DealerId == dealerId && po.IsActive)
                .ToListAsync();
        }
    
    }
}
