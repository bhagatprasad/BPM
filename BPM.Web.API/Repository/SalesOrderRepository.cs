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
            return await _dbContext.SalesOrders.AsNoTracking().Include(x => x.PurchaseOrder).ThenInclude(x => x.Dealer)
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.PurchaseOrderItems)
                .Include(x => x.Dealer)
                .Include(x => x.Supplier)
                .Include(x => x.SalesOrderItems).ThenInclude(x => x.Drug).OrderByDescending(x => x.ModifiedOn)
                .ToListAsync();
        }

        public async Task<SalesOrder?> GetSalesOrderByIdAsync(Guid id)
        {
            return await _dbContext.SalesOrders
                .AsNoTracking()
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.Dealer)
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.PurchaseOrderItems)
                .Include(x => x.Dealer)
                .Include(x => x.Supplier)
                .Include(x => x.SalesOrderItems).ThenInclude(x => x.Drug)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SalesOrder>> GetSalesOrderByDealer(Guid dealerId)
        {
            return await _dbContext.SalesOrders
                .AsNoTracking()
                .Where(x => x.DealerId == dealerId)
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.Dealer)
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.PurchaseOrderItems)
                .Include(x => x.Dealer)
                .Include(x => x.Supplier)
                .Include(x => x.SalesOrderItems).ThenInclude(x => x.Drug).OrderByDescending(x => x.ModifiedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalesOrder>> GetSalesOrderByPurchaseOrder(Guid purchaseOrderId)
        {
            return await _dbContext.SalesOrders
                .AsNoTracking()
                .Where(x => x.PurchaseOrderId == purchaseOrderId)
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.Dealer)
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.PurchaseOrderItems)
                .Include(x => x.Dealer)
                .Include(x => x.Supplier)
                .Include(x => x.SalesOrderItems).ThenInclude(x => x.Drug).OrderByDescending(x => x.ModifiedOn)
                .ToListAsync();
        }

        public async Task<SalesOrder> CreateSalesOrderAsync(SalesOrder salesOrder)
        {
            if (salesOrder.Id == Guid.Empty)
            {
                salesOrder.Id = Guid.NewGuid();
            }

            salesOrder.CreatedOn = DateTime.UtcNow;
            salesOrder.IsActive = true;

            if (salesOrder.SalesOrderItems != null &&
                salesOrder.SalesOrderItems.Any())
            {
                foreach (var item in salesOrder.SalesOrderItems)
                {
                    if (item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid();
                    }

                    item.SalesOrderId = salesOrder.Id;
                    item.CreatedOn = DateTime.UtcNow;

                    item.PendingQuantity =
                        item.Quantity - item.ReceivedQuantity;
                }
            }

            await _dbContext.SalesOrders.AddAsync(salesOrder);
            await _dbContext.SaveChangesAsync();

            return salesOrder;
        }

        public async Task<SalesOrder?> UpdateSalesOrderAsync(SalesOrder salesOrder)
        {
            var existingOrder = await _dbContext.SalesOrders
                .FirstOrDefaultAsync(x => x.Id == salesOrder.Id);

            if (existingOrder == null)
                return null;

            existingOrder.Status = salesOrder.Status;
            existingOrder.ExpectedDeliveryDate = salesOrder.ExpectedDeliveryDate;
            existingOrder.ActualDeliveryDate = salesOrder.ActualDeliveryDate;

            existingOrder.SubTotal = salesOrder.SubTotal;
            existingOrder.TaxAmount = salesOrder.TaxAmount;
            existingOrder.DiscountAmount = salesOrder.DiscountAmount;
            existingOrder.TotalAmount = salesOrder.TotalAmount;

            existingOrder.PaymentTerms = salesOrder.PaymentTerms;
            existingOrder.DeliveryTerms = salesOrder.DeliveryTerms;
            existingOrder.Remarks = salesOrder.Remarks;
            existingOrder.InternalNotes = salesOrder.InternalNotes;

            existingOrder.ModifiedBy = salesOrder.ModifiedBy;
            existingOrder.ModifiedOn = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<bool> DeleteSalesOrderAsync(Guid id)
        {
            var salesOrder = await _dbContext.SalesOrders
                .FirstOrDefaultAsync(x => x.Id == id);

            if (salesOrder == null)
                return false;

            salesOrder.IsActive = false;
            salesOrder.ModifiedOn = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<SalesOrder> ProcessSalesOrderAsync(Guid salesOrderId, string status)
        {
            var salesOrder = await _dbContext.SalesOrders.FirstOrDefaultAsync(a => a.Id == salesOrderId);

            if (salesOrder == null)
            {
                throw new KeyNotFoundException("Sales Order not found.");
            }

            salesOrder.Status = status;

            await _dbContext.SaveChangesAsync();

            return salesOrder;
        }
    }
}