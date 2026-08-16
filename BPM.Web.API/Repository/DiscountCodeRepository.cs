using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repositories
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiscountCode> CreateAsync(DiscountCode discountCode)
        {
            await _context.DiscountCodes.AddAsync(discountCode);
            await _context.SaveChangesAsync();

            return discountCode;
        }

        public async Task<IEnumerable<DiscountCode>> GetAllAsync()
        {
            return await _context.DiscountCodes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<DiscountCode?> GetByIdAsync(Guid id)
        {
            return await _context.DiscountCodes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<DiscountCode?> GetByCodeAsync(string discountCode)
        {
            return await _context.DiscountCodes
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.DiscountCodeValue == discountCode &&
                    x.IsActive);
        }

        public async Task<IEnumerable<DiscountCode>> GetBySupplierAsync(Guid supplierId)
        {
            return await _context.DiscountCodes
                .AsNoTracking()
                .Where(x => x.SupplierId == supplierId && x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<DiscountCode>> GetActiveCodesAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.DiscountCodes
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    x.StartDate <= now &&
                    x.ExpiryDate >= now &&
                    (!x.RequiresApproval || x.IsApproved))
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }
    }
}
