using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
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
                .OrderBy(x => x.Drug!.DrugName)
                .ThenBy(x => x.PackageUom!.UomName)
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
                .Where(x => x.DrugId == drugId && x.IsActive)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .OrderBy(x => x.PackageUom!.UomName)
                .ToListAsync();
        }

        public async Task<List<DrugPackaging>> GetByPackageUomIdAsync(Guid packageUomId)
        {
            return await _context.DrugPackagings
                .Where(x => x.PackageUomId == packageUomId && x.IsActive)
                .Include(x => x.Drug)
                .Include(x => x.ContainsUom)
                .OrderBy(x => x.Drug!.DrugName)
                .ToListAsync();
        }

        public async Task<List<DrugPackaging>> GetByContainsUomIdAsync(Guid containsUomId)
        {
            return await _context.DrugPackagings
                .Where(x => x.ContainsUomId == containsUomId && x.IsActive)
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .OrderBy(x => x.Drug!.DrugName)
                .ToListAsync();
        }

        public async Task<DrugPackaging?> GetByBarcodeAsync(string barcode)
        {
            return await _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .FirstOrDefaultAsync(x => x.Barcode == barcode && x.IsActive);
        }

        public async Task<List<DrugPackaging>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.DrugPackagings
                .Where(x => x.PackagePrice >= minPrice && x.PackagePrice <= maxPrice && x.IsActive)
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .OrderBy(x => x.PackagePrice)
                .ToListAsync();
        }

        public async Task<(List<DrugPackaging> Items, int TotalCount)> GetFilteredAsync(DrugPackagingFilterDto filter)
        {
            var query = _context.DrugPackagings
                .Include(x => x.Drug)
                .Include(x => x.PackageUom)
                .Include(x => x.ContainsUom)
                .AsQueryable();

            // Apply filters
            if (filter.DrugId.HasValue)
                query = query.Where(x => x.DrugId == filter.DrugId.Value);

            if (filter.PackageUomId.HasValue)
                query = query.Where(x => x.PackageUomId == filter.PackageUomId.Value);

            if (filter.ContainsUomId.HasValue)
                query = query.Where(x => x.ContainsUomId == filter.ContainsUomId.Value);

            if (!string.IsNullOrEmpty(filter.Barcode))
                query = query.Where(x => x.Barcode != null && x.Barcode.Contains(filter.Barcode));

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(x => x.PackagePrice >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(x => x.PackagePrice <= filter.MaxPrice.Value);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "drugname" => filter.SortDescending
                        ? query.OrderByDescending(x => x.Drug!.DrugName)
                        : query.OrderBy(x => x.Drug!.DrugName),
                    "packageprice" => filter.SortDescending
                        ? query.OrderByDescending(x => x.PackagePrice)
                        : query.OrderBy(x => x.PackagePrice),
                    "unitprice" => filter.SortDescending
                        ? query.OrderByDescending(x => x.UnitPrice)
                        : query.OrderBy(x => x.UnitPrice),
                    "quantity" => filter.SortDescending
                        ? query.OrderByDescending(x => x.Quantity)
                        : query.OrderBy(x => x.Quantity),
                    _ => filter.SortDescending
                        ? query.OrderByDescending(x => x.CreatedOn)
                        : query.OrderBy(x => x.CreatedOn)
                };
            }
            else
            {
                query = query.OrderBy(x => x.Drug!.DrugName).ThenBy(x => x.PackageUom!.UomName);
            }

            // Apply pagination
            var pageSize = filter.PageSize ?? 10;
            var page = filter.Page ?? 1;
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> InsertAsync(DrugPackaging packaging)
        {
            packaging.PackagingId = Guid.NewGuid();
            packaging.CreatedOn = DateTime.UtcNow;
            packaging.IsActive = true;

            await _context.DrugPackagings.AddAsync(packaging);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(DrugPackaging packaging)
        {
            var existing = await _context.DrugPackagings
                .FirstOrDefaultAsync(x => x.PackagingId == packaging.PackagingId);

            if (existing == null)
                return false;

            existing.DrugId = packaging.DrugId;
            existing.PackageUomId = packaging.PackageUomId;
            existing.ContainsUomId = packaging.ContainsUomId;
            existing.Quantity = packaging.Quantity;
            existing.TotalUnits = packaging.TotalUnits;
            existing.UnitPrice = packaging.UnitPrice;
            existing.PackagePrice = packaging.PackagePrice;
            existing.Barcode = packaging.Barcode;
            existing.GrossWeight = packaging.GrossWeight;
            existing.NetWeight = packaging.NetWeight;
            existing.Length = packaging.Length;
            existing.Width = packaging.Width;
            existing.Height = packaging.Height;
            existing.IsActive = packaging.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid packagingId)
        {
            var existing = await _context.DrugPackagings
                .FirstOrDefaultAsync(x => x.PackagingId == packagingId);

            if (existing == null)
                return false;

            // Soft delete
            existing.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByBarcodeAsync(string barcode, Guid? excludeId = null)
        {
            var query = _context.DrugPackagings
                .Where(x => x.Barcode != null && x.Barcode.ToLower() == barcode.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.PackagingId != excludeId.Value);

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
                .SumAsync(x => x.Quantity);
        }

        public async Task<bool> ValidateUomCompatibilityAsync(Guid packageUomId, Guid containsUomId)
        {
            // Check if both UOMs belong to the same drug (this validation should be done at service level)
            // Additional validation: ensure contains UOM is a smaller unit than package UOM
            var packageUom = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == packageUomId && x.IsActive);

            var containsUom = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == containsUomId && x.IsActive);

            if (packageUom == null || containsUom == null)
                return false;

            // Package UOM should have higher conversion factor than contains UOM
            // This ensures package contains multiple units
            return packageUom.ConversionFactor >= containsUom.ConversionFactor;
        }
    }
}
