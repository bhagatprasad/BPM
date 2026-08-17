using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs.PurchaseOrder;
using BPM.Web.API.Models.Entities;
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
            return await _dbContext.PurchaseOrders.Where(po => po.IsActive).Include(po => po.Supplier).Include(x => x.Dealer).Include(po => po.PurchaseOrderItems).ThenInclude(item => item.Drug).OrderByDescending(po => po.ModifiedOn).ToListAsync();
        }


        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid id)
        {
            return await _dbContext.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Dealer)
                .Include(po => po.PurchaseOrderItems)
                .ThenInclude(item => item.Drug)
                .FirstOrDefaultAsync(po => po.Id == id && po.IsActive);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByDealerAsync(Guid dealerId)
        {
            return await _dbContext.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Dealer)
                .Include(po => po.PurchaseOrderItems)
                .ThenInclude(item => item.Drug)
                .Where(po => po.DealerId == dealerId && po.IsActive && po.Status != "Draft")
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();
        }

        public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _dbContext.PurchaseOrders.Update(purchaseOrder);
            await _dbContext.SaveChangesAsync();
            return purchaseOrder;
        }

        public async Task<ProductAvailabilityResponseDto> ValidateProductAvailabilityAsync(Guid drugId, Guid packagingId, int quantity)
        {
            var inventory = await _dbContext.Inventories.AsNoTracking().FirstOrDefaultAsync(x => x.DrugId == drugId && x.PackagingId == packagingId && x.IsActive);
            //// Read-only query; entity tracking is not required.
            if (inventory == null)
            {
                return new ProductAvailabilityResponseDto
                {
                    DrugId = drugId,
                    PackagingId = packagingId,
                    RequestedQuantity = quantity,
                    AvailableQuantity = 0,
                    IsAvailable = false,
                    Message = "Product is not available in inventory."
                };
            }

            return new ProductAvailabilityResponseDto
            {
                DrugId = drugId,
                PackagingId = packagingId,
                RequestedQuantity = quantity,
                AvailableQuantity = inventory.AvailableQuantity,
                IsAvailable = inventory.AvailableQuantity >= quantity,
                Message = inventory.AvailableQuantity >= quantity ? "Product is available." : "Insufficient stock available."
            };
        }

        public async Task<PurchaseOrder> SubmitPurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _dbContext.PurchaseOrders.Update(purchaseOrder);
            await _dbContext.SaveChangesAsync();
            return purchaseOrder;
        }

        public async Task<PurchaseOrder> SavePurchaseOrderDraftAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> purchaseOrderItems)
        {
            if (purchaseOrder.Id == Guid.Empty)
            {
                await _dbContext.PurchaseOrders.AddAsync(purchaseOrder);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.PurchaseOrders.Update(purchaseOrder);
                await _dbContext.SaveChangesAsync();

                var existingItems = await _dbContext.PurchaseOrderItems.Where(x => x.PurchaseOrderId == purchaseOrder.Id).ToListAsync();

                if (existingItems.Any())
                {
                    _dbContext.PurchaseOrderItems.RemoveRange(existingItems);
                    await _dbContext.SaveChangesAsync();
                }
            }

            foreach (var item in purchaseOrderItems)
            {
                item.PurchaseOrderId = purchaseOrder.Id;
            }

            if (purchaseOrderItems.Any())
            {
                await _dbContext.PurchaseOrderItems.AddRangeAsync(purchaseOrderItems);
                await _dbContext.SaveChangesAsync();
            }

            return purchaseOrder;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetDraftPurchaseOrdersAsync(Guid dealerId)
        {
            return await _dbContext.PurchaseOrders.Where(po => po.DealerId == dealerId && po.IsActive && po.Status == "Draft").Include(po => po.Supplier).Include(po => po.Dealer).Include(po => po.PurchaseOrderItems).ThenInclude(item => item.Drug).OrderByDescending(po => po.ModifiedOn).ToListAsync();
        }

        public async Task<bool> DeletePurchaseOrderDraftAsync(Guid purchaseOrderId)
        {
            var purchaseOrder = await _dbContext.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == purchaseOrderId && x.IsActive && x.Status == "Draft");

            if (purchaseOrder == null)
            {
                return false;
            }

            purchaseOrder.IsActive = false;
            purchaseOrder.ModifiedOn = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetActiveDraftCountAsync(Guid dealerId)
        {
            return await _dbContext.PurchaseOrders.CountAsync(x => x.DealerId == dealerId && x.IsActive && x.Status == "Draft");
        }

        public async Task<int> DeleteExpiredDraftPurchaseOrdersAsync()
        {
            var expiryDate = DateTime.UtcNow.AddDays(-30);
            var expiredDrafts = await _dbContext.PurchaseOrders.Where(x => x.Status == "Draft" && x.IsActive && x.OrderDate < expiryDate).ToListAsync();

            if (!expiredDrafts.Any())
            {
                return 0;
            }

            foreach (var purchaseOrder in expiredDrafts)
            {
                purchaseOrder.IsActive = false;
                purchaseOrder.ModifiedOn = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync();
            return expiredDrafts.Count;
        }
    }
}