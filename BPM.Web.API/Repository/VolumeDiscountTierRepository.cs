using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repositories
{
    public class VolumeDiscountTierRepository : IVolumeDiscountTierRepository
    {
        private readonly ApplicationDbContext _context;

        public VolumeDiscountTierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VolumeDiscountTier> CreateAsync(VolumeDiscountTier volumeDiscountTier)
        {
            await _context.VolumeDiscountTiers.AddAsync(volumeDiscountTier);
            await _context.SaveChangesAsync();

            return volumeDiscountTier;
        }

        public async Task<IEnumerable<VolumeDiscountTier>> GetAllAsync()
        {
            return await _context.VolumeDiscountTiers
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.MinQuantity)
                .ToListAsync();
        }

        public async Task<VolumeDiscountTier?> GetByIdAsync(Guid id)
        {
            return await _context.VolumeDiscountTiers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<IEnumerable<VolumeDiscountTier>> GetBySupplierAsync(Guid supplierId)
        {
            return await _context.VolumeDiscountTiers
                .AsNoTracking()
                .Where(x => x.SupplierId == supplierId && x.IsActive)
                .OrderBy(x => x.MinQuantity)
                .ToListAsync();
        }
    }
}
