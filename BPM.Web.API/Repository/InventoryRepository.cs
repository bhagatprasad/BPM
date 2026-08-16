using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.OrderBy(i => i.CreatedOn).ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(Guid id)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Inventory>> GetByDistributorIdAsync(Guid distributorId)
        {
            return await _context.Inventories.Where(i => i.DistributorId == distributorId).OrderBy(i => i.CreatedOn).ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetByDrugIdAsync(Guid drugId)
        {
            return await _context.Inventories.Where(i => i.DrugId == drugId).OrderBy(i => i.CreatedOn).ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetByWarehouseIdAsync(Guid warehouseId)
        {
            return await _context.Inventories.Where(i => i.WarehouseId == warehouseId).OrderBy(i => i.CreatedOn).ToListAsync();
        }

        public async Task<Inventory?> GetInventoryForAvailabilityAsync(Guid drugId, Guid packagingId, Guid batchId, Guid warehouseId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.DrugId == drugId && i.PackagingId == packagingId && i.BatchId == batchId && i.WarehouseId == warehouseId && i.IsActive);
        }

        public async Task<bool> UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
                return false;

            inventory.IsActive = false;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}