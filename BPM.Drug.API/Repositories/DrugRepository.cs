using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.Drug.API.Repositories
{
    public class DrugRepository : IDrugRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DrugRepository(ApplicationDbContext applicationDbContext) 
        {
            _dbContext = applicationDbContext;
        }
        public async Task<bool> DeleteDrugAsync(Guid drugId)
        {
           var drug= await _dbContext.Drugs.FirstOrDefaultAsync(a=>a.DrugId== drugId);
            if (drug==null)
            {
                return false;
            }
            drug.IsActive = false;
            return await _dbContext.SaveChangesAsync()>0;

        }

        public async Task<List<DrugEntity>> GetAllDrugsAsync()
        {
            return await _dbContext.Drugs.OrderBy(a=>a.DrugName).ToListAsync();
        }

        public async Task<DrugEntity?> GetDrugByIdAsync(Guid drugId)
        {
            return await _dbContext.Drugs.FirstOrDefaultAsync(a=>a.DrugId== drugId);
        }

        public async Task<bool> InsertDrugAsync(DrugEntity drug)
        {
            drug.DrugId = Guid.NewGuid();
            drug.IsActive=true;
            drug.CreatedOn = DateTime.UtcNow;

            await _dbContext.Drugs.AddAsync(drug);
            return await _dbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> UpdateDrugAsync(DrugEntity drug)
        {
           var existing= await _dbContext.Drugs.FirstOrDefaultAsync(a=>a.DrugId == drug.DrugId);
            if (existing==null)
            {
                return false;
            }
            existing.DrugName = drug.DrugName;
            existing.DrugCode= drug.DrugCode;
            existing.GenericName= drug.GenericName;
            existing.BrandName= drug.BrandName;
            existing.Manufacturer = drug.Manufacturer;
            existing.Category=drug.Category;
            existing.HsnCode=drug.HsnCode;
            existing.ScheduleType=drug.ScheduleType;
            existing.Packing=drug.Packing;
            existing.Strength=drug.Strength;
            existing.IsActive=drug.IsActive;    
            existing.ModifiedBy=drug.ModifiedBy;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
