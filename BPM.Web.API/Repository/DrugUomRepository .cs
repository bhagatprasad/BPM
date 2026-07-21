using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class DrugUomRepository : IDrugUomRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugUomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DrugUom>> GetAllAsync()
        {
            return await _context.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .OrderBy(x => x.DrugId)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        public async Task<DrugUom?> GetByIdAsync(Guid uomId)
        {
            return await _context.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .FirstOrDefaultAsync(x => x.UomId == uomId);
        }

        public async Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId)
        {
            return await _context.DrugUoms
                .Where(x => x.DrugId == drugId)
                .Include(x => x.ParentUom)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        public async Task<DrugUom?> GetByCodeAsync(Guid drugId, string uomCode)
        {
            return await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.DrugId == drugId && x.UomCode == uomCode);
        }

        public async Task<List<DrugUom>> GetBaseUnitsByDrugIdAsync(Guid drugId)
        {
            return await _context.DrugUoms
                .Where(x => x.DrugId == drugId && x.IsBaseUnit && x.IsActive)
                .OrderBy(x => x.UomName)
                .ToListAsync();
        }

        public async Task<List<DrugUom>> GetPurchaseUomsByDrugIdAsync(Guid drugId)
        {
            return await _context.DrugUoms
                .Where(x => x.DrugId == drugId && x.IsPurchaseUom && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        public async Task<List<DrugUom>> GetSalesUomsByDrugIdAsync(Guid drugId)
        {
            return await _context.DrugUoms
                .Where(x => x.DrugId == drugId && x.IsSalesUom && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        public async Task<bool> InsertAsync(DrugUom drugUom)
        {
            drugUom.UomId = Guid.NewGuid();
            drugUom.CreatedOn = DateTime.UtcNow;
            drugUom.IsActive = true;

            await _context.DrugUoms.AddAsync(drugUom);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(DrugUom drugUom)
        {
            var existing = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == drugUom.UomId);

            if (existing == null)
                return false;

            existing.DrugId = drugUom.DrugId;
            existing.UomCode = drugUom.UomCode;
            existing.UomName = drugUom.UomName;
            existing.UomType = drugUom.UomType;
            existing.ParentUomId = drugUom.ParentUomId;
            existing.QuantityPerParent = drugUom.QuantityPerParent;
            existing.ConversionFactor = drugUom.ConversionFactor;
            existing.IsBaseUnit = drugUom.IsBaseUnit;
            existing.IsPurchaseUom = drugUom.IsPurchaseUom;
            existing.IsSalesUom = drugUom.IsSalesUom;
            existing.IsInventoryUom = drugUom.IsInventoryUom;
            existing.DisplayOrder = drugUom.DisplayOrder;
            existing.Remarks = drugUom.Remarks;
            existing.IsActive = drugUom.IsActive;
            existing.ModifiedBy = drugUom.ModifiedBy;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid uomId)
        {
            var existing = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == uomId);

            if (existing == null)
                return false;

            // Check if this UOM has child UOMs
            if (await HasChildUomsAsync(uomId))
                throw new InvalidOperationException("Cannot delete UOM that has child UOMs.");

            _context.DrugUoms.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(Guid drugId, string uomCode, Guid? excludeUomId = null)
        {
            var query = _context.DrugUoms
                .Where(x => x.DrugId == drugId && x.UomCode.ToLower() == uomCode.ToLower());

            if (excludeUomId.HasValue)
                query = query.Where(x => x.UomId != excludeUomId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> HasChildUomsAsync(Guid parentUomId)
        {
            return await _context.DrugUoms
                .AnyAsync(x => x.ParentUomId == parentUomId);
        }
    }
}