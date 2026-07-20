using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class PackagingMasterRepository : IPackagingMasterRepository
    {
        private readonly ApplicationDbContext _context;

        public PackagingMasterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PackagingMaster>> GetAllAsync()
        {
            return await _context.PackagingMasters
                .OrderBy(x => x.PackagingName)
                .ToListAsync();
        }

        public async Task<PackagingMaster?> GetByIdAsync(Guid packagingId)
        {
            return await _context.PackagingMasters
                .FirstOrDefaultAsync(x => x.PackagingId == packagingId);
        }

        public async Task<bool> InsertAsync(PackagingMaster packaging)
        {
            packaging.PackagingId = Guid.NewGuid();
            packaging.CreatedOn = DateTime.UtcNow;
            packaging.IsActive = true;

            await _context.PackagingMasters.AddAsync(packaging);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(PackagingMaster packaging)
        {
            var existing = await _context.PackagingMasters
                .FirstOrDefaultAsync(x => x.PackagingId == packaging.PackagingId);

            if (existing == null)
                return false;

            existing.PackagingCode = packaging.PackagingCode;
            existing.PackagingName = packaging.PackagingName;
            existing.Description = packaging.Description;
            existing.ContainsQuantity = packaging.ContainsQuantity;
            existing.UomId = packaging.UomId;
            existing.IsActive = packaging.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid packagingId)
        {
            var existing = await _context.PackagingMasters
                .FirstOrDefaultAsync(x => x.PackagingId == packagingId);

            if (existing == null)
                return false;

            _context.PackagingMasters.Remove(existing);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}