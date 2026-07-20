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
                .OrderBy(x => x.UomName)
                .ToListAsync();
        }

        public async Task<DrugUom?> GetByIdAsync(Guid uomId)
        {
            return await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == uomId);
        }

        public async Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId)
        {
            return await _context.DrugUoms
                .Where(x => x.DrugId == drugId)
                .OrderBy(x => x.UomName)
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
            existing.ConversionFactor = drugUom.ConversionFactor;
            existing.IsBaseUnit = drugUom.IsBaseUnit;
            existing.IsActive = drugUom.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid uomId)
        {
            var existing = await _context.DrugUoms
                .FirstOrDefaultAsync(x => x.UomId == uomId);

            if (existing == null)
                return false;

            _context.DrugUoms.Remove(existing);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}