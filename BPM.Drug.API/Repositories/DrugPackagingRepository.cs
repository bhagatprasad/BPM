using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.Drug.API.Repositories
{
    public class DrugPackagingRepository : IDrugPackagingRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugPackagingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DrugPackaging>> GetAllAsync()
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .OrderBy(x => x.Drug.DrugName)
                .ThenBy(x => x.PackageUom.DisplayOrder)
                .ToListAsync();
        }

        public async Task<DrugPackaging?> GetByIdAsync(Guid packagingId)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .FirstOrDefaultAsync(x => x.PackagingId == packagingId);
        }

        public async Task<List<DrugPackaging>> GetByDrugIdAsync(Guid drugId)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .Where(x => x.DrugId == drugId)
                .OrderBy(x => x.PackageUom.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<DrugPackaging>> GetByPackageUomIdAsync(Guid packageUomId)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .Where(x => x.PackageUomId == packageUomId)
                .ToListAsync();
        }

        public async Task<List<DrugPackaging>> GetByContainsUomIdAsync(Guid containsUomId)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .Where(x => x.ContainsUomId == containsUomId)
                .ToListAsync();
        }

        public async Task<DrugPackaging?> GetByBarcodeAsync(string barcode)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .FirstOrDefaultAsync(x => x.Barcode == barcode);
        }

        public async Task<List<DrugPackaging>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .Where(x => x.PackagePrice >= minPrice && x.PackagePrice <= maxPrice)
                .OrderBy(x => x.PackagePrice)
                .ToListAsync();
        }

        public async Task<(List<DrugPackaging> Items, int TotalCount)> GetFilteredAsync(DrugPackagingDto.DrugPackagingFilterDto filter)
        {
            var query = _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .AsQueryable();

            if (filter.DrugId.HasValue)
            {
                query = query.Where(x => x.DrugId == filter.DrugId.Value);
            }

            if (filter.PackageUomId.HasValue)
            {
                query = query.Where(x => x.PackageUomId == filter.PackageUomId.Value);
            }

            if (filter.ContainsUomId.HasValue)
            {
                query = query.Where(x => x.ContainsUomId == filter.ContainsUomId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Barcode))
            {
                query = query.Where(x => x.Barcode != null && x.Barcode.Contains(filter.Barcode));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(x => x.PackagePrice >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.PackagePrice <= filter.MaxPrice.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync();

            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;

            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

            var items = await query
                .OrderBy(x => x.Drug.DrugName)
                .ThenBy(x => x.PackageUom.DisplayOrder)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> InsertAsync(DrugPackaging packaging)
        {
            await _context.DrugPackagings.AddAsync(packaging);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(DrugPackaging packaging)
        {
            _context.DrugPackagings.Update(packaging);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid packagingId)
        {
            var packaging = await _context.DrugPackagings
                .FirstOrDefaultAsync(x => x.PackagingId == packagingId);

            if (packaging == null)
            {
                return false;
            }

            packaging.IsActive = false;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByBarcodeAsync(string barcode, Guid? excludeId = null)
        {
            var query = _context.DrugPackagings
                .Where(x => x.Barcode == barcode);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.PackagingId != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasActivePackagingAsync(Guid drugId)
        {
            return await _context.DrugPackagings
                .AnyAsync(x => x.DrugId == drugId && x.IsActive);
        }

        public async Task<decimal> GetTotalPackagesByDrugAsync(Guid drugId)
        {
            return await _context.DrugPackagings
                .Where(x => x.DrugId == drugId && x.IsActive)
                .SumAsync(x => (decimal?)x.TotalUnits) ?? 0;
        }

        public async Task<bool> ValidateUomCompatibilityAsync(Guid packageUomId, Guid containsUomId)
        {
            if (packageUomId == containsUomId)
            {
                return false;
            }

            var packageUom = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == packageUomId && x.IsActive);

            var containsUom = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == containsUomId && x.IsActive);

            if (packageUom == null || containsUom == null)
            {
                return false;
            }

            return packageUom.DrugId == containsUom.DrugId;
        }
    }
}
