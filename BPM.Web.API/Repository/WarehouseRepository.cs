using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Warehouse> CreateAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }

        public async Task<List<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses.OrderBy(w => w.WarehouseName).ToListAsync();
        }

        public async Task<Warehouse?> GetByIdAsync(Guid id)
        {
            return await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Warehouse?> GetByCodeAsync(string warehouseCode)
        {
            return await _context.Warehouses.FirstOrDefaultAsync(w => w.WarehouseCode == warehouseCode);
        }

        public async Task<List<Warehouse>> GetByDistributorIdAsync(Guid distributorId)
        {
            return await _context.Warehouses.Where(w => w.DistributorId == distributorId).OrderBy(w => w.WarehouseName).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
            {
                return false;
            }

            warehouse.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}