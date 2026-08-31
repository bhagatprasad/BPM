using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.Drug.API.Repositories
{
    public class DrugUomRepository : IDrugUomRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DrugUomRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        // GET ALL DRUG UOMS
        public async Task<List<DrugUom>> GetAllDrugUomsAsync()
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .OrderBy(x => x.DrugId)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        // GET DRUG UOM BY ID
        public async Task<DrugUom?> GetDrugUomByIdAsync(Guid uomId)
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .FirstOrDefaultAsync(x => x.UomId == uomId);
        }

        // GET DRUG UOMS BY DRUG ID
        public async Task<List<DrugUom>> GetDrugUomsByDrugIdAsync(Guid drugId)
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .Where(x => x.DrugId == drugId)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        // GET DRUG UOM BY CODE
        public async Task<DrugUom?> GetDrugUomByCodeAsync(Guid drugId,
            string uomCode)
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .FirstOrDefaultAsync(x =>
                    x.DrugId == drugId &&
                    x.UomCode == uomCode);
        }

        // GET BASE UNITS BY DRUG ID
        public async Task<List<DrugUom>> GetBaseUnitsByDrugIdAsync(Guid drugId)
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Where(x =>
                    x.DrugId == drugId &&
                    x.IsBaseUnit &&
                    x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        // GET PURCHASE UOMS BY DRUG ID
        public async Task<List<DrugUom>> GetPurchaseUomsByDrugIdAsync(Guid drugId)
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .Where(x =>
                    x.DrugId == drugId &&
                    x.IsPurchaseUom &&
                    x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        // GET SALES UOMS BY DRUG ID
        public async Task<List<DrugUom>> GetSalesUomsByDrugIdAsync(Guid drugId)
        {
            return await _dbContext.DrugUoms
                .Include(x => x.Drug)
                .Include(x => x.ParentUom)
                .Where(x =>
                    x.DrugId == drugId &&
                    x.IsSalesUom &&
                    x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.UomName)
                .ToListAsync();
        }

        // INSERT DRUG UOM
        public async Task<bool> InsertDrugUomAsync(DrugUom drugUom)
        {
            await _dbContext.DrugUoms.AddAsync(drugUom);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        // UPDATE DRUG UOM
        public async Task<bool> UpdateDrugUomAsync(DrugUom drugUom)
        {
            var existing = await _dbContext.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == drugUom.UomId);

            if (existing == null)
            {
                return false;
            }

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

            return await _dbContext.SaveChangesAsync() > 0;
        }

        // SOFT DELETE DRUG UOM
        public async Task<bool> DeleteDrugUomAsync(Guid uomId)
        {
            var drugUom = await _dbContext.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == uomId);

            if (drugUom == null)
            {
                return false;
            }

            drugUom.IsActive = false;
            drugUom.ModifiedOn = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        // CHECK DUPLICATE UOM CODE
        public async Task<bool> DrugUomExistsAsync(Guid drugId, string uomCode,Guid? excludeUomId = null)
        {
            return await _dbContext.DrugUoms
                .AnyAsync(x =>
                    x.DrugId == drugId &&
                    x.UomCode == uomCode &&
                    (!excludeUomId.HasValue ||
                     x.UomId != excludeUomId.Value));
        }

        // CHECK CHILD UOMS
        public async Task<bool> HasChildUomsAsync(Guid parentUomId)
        {
            return await _dbContext.DrugUoms.AnyAsync(x => x.ParentUomId == parentUomId);
        }
    }
}
