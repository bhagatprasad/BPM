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
        public async Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
         await _dbContext.PurchaseOrders.AddAsync(purchaseOrder);
         await _dbContext.SaveChangesAsync();
        return purchaseOrder;
        }
    }
}
