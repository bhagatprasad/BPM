using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repositories
{
    public class SupplierDiscountRepository : ISupplierDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierDiscountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierDiscount> CreateAsync(SupplierDiscount supplierDiscount)
        {
            await _context.SupplierDiscounts.AddAsync(supplierDiscount);
            await _context.SaveChangesAsync();

            return supplierDiscount;
        }

        public async Task<IEnumerable<SupplierDiscount>> GetAllAsync()
        {
            return await _context.SupplierDiscounts
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<SupplierDiscount?> GetByIdAsync(Guid id)
        {
            return await _context.SupplierDiscounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<IEnumerable<SupplierDiscount>> GetBySupplierAsync(Guid supplierId)
        {
            return await _context.SupplierDiscounts
                .AsNoTracking()
                .Where(x => x.SupplierId == supplierId && x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }
    }
}