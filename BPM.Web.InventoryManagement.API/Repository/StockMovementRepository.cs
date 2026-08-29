using BPM.Web.InventoryManagement.API.Models.Data;
using BPM.Web.InventoryManagement.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.InventoryManagement.API.Repository
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly ApplicationDbContext _context;

        public StockMovementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockMovement> CreateAsync(StockMovement stockMovement)
        {
            await _context.StockMovements.AddAsync(stockMovement);
            await _context.SaveChangesAsync();

            return stockMovement;
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            return await _context.StockMovements
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<StockMovement?> GetByIdAsync(Guid id)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<StockMovement>> GetByInventoryAsync(Guid inventoryId)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetByDrugAsync(Guid drugId)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.DrugId == drugId)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetByWarehouseAsync(Guid warehouseId)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.WarehouseId == warehouseId)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetByDistributorAsync(Guid distributorId)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.DistributorId == distributorId)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<StockMovement?> UpdateAsync(StockMovement stockMovement)
        {
            var existing = await _context.StockMovements.FindAsync(stockMovement.Id);
            if (existing == null)
            {
                return null;
            }

            _context.Entry(existing).CurrentValues.SetValues(stockMovement);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement == null)
            {
                return false;
            }

            _context.StockMovements.Remove(stockMovement);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<StockMovement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetByMovementTypeAsync(string movementType)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.MovementType == movementType)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalQuantityByInventoryAsync(Guid inventoryId)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .SumAsync(x => x.Quantity);
        }

        public async Task<IEnumerable<StockMovement>> GetByInventoryAndDateRangeAsync(Guid inventoryId, DateTime startDate, DateTime endDate)
        {
            return await _context.StockMovements
                .AsNoTracking()
                .Where(x => x.InventoryId == inventoryId && x.CreatedOn >= startDate && x.CreatedOn <= endDate)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }
    }
}