using BPM.Web.Drug.API.Models.Data;
using Microsoft.EntityFrameworkCore;
using DrugEntity = BPM.Web.Drug.API.Models.Entities.Drug;

namespace BPM.Web.Drug.API.Repositories
{
    public class DrugRepository : IDrugRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DrugRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        // SOFT DELETE
        public async Task<bool> DeleteDrugAsync(Guid drugId)
        {
            var drug = await _dbContext.Drugs
                .FirstOrDefaultAsync(a => a.DrugId == drugId);

            if (drug == null)
            {
                return false;
            }

            drug.IsActive = false;
            drug.ModifiedOn = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        // GET ALL
        public async Task<List<DrugEntity>> GetAllDrugsAsync()
        {
            return await _dbContext.Drugs
                .Include(x => x.DrugForm)
                .Include(x=>x.DrugUoms)
                .Include(x=>x.DrugPackagings)
                .OrderBy(a => a.DrugName)
                .ToListAsync();
        }

        // GET BY ID
        public async Task<DrugEntity?> GetDrugByIdAsync(Guid drugId)
        {
            return await _dbContext.Drugs
                .Include(x => x.DrugForm)
                .Include(x=>x.DrugUoms)
                .Include(x=>x.DrugPackagings)
                .FirstOrDefaultAsync(a => a.DrugId == drugId);
        }

        // INSERT
        public async Task<bool> InsertDrugAsync(DrugEntity drug)
        {
            await _dbContext.Drugs.AddAsync(drug);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        // UPDATE
        public async Task<bool> UpdateDrugAsync(DrugEntity drug)
        {
            var existing = await _dbContext.Drugs
                .FirstOrDefaultAsync(a => a.DrugId == drug.DrugId);

            if (existing == null)
            {
                return false;
            }

            existing.FormId = drug.FormId;
            existing.DrugName = drug.DrugName;
            existing.DrugCode = drug.DrugCode;
            existing.GenericName = drug.GenericName;
            existing.BrandName = drug.BrandName;
            existing.Manufacturer = drug.Manufacturer;
            existing.Category = drug.Category;
            existing.HsnCode = drug.HsnCode;
            existing.ScheduleType = drug.ScheduleType;
            existing.Packing = drug.Packing;
            existing.Strength = drug.Strength;
            existing.IsActive = drug.IsActive;
            existing.ModifiedBy = drug.ModifiedBy;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}