using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.Drug.API.Repositories
{
    public class DrugCategoryRepository : IDrugCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DrugCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<List<DrugCategory>> GetAllDrugCategoriesAsync()
        {
            return await _dbContext.DrugCategories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<DrugCategory?> GetDrugCategoryByIdAsync(Guid id)
        {
            return await _dbContext.DrugCategories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> InsertDrugCategoryAsync(DrugCategory drugCategory)
        {
            drugCategory.CreatedOn = DateTime.UtcNow;
            await _dbContext.DrugCategories.AddAsync(drugCategory);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDrugCategoryAsync(DrugCategory drugCategory)
        {
            var existing = await _dbContext.DrugCategories
                .FirstOrDefaultAsync(x => x.Id == drugCategory.Id);

            if (existing == null)
            {
                return false;
            }

            existing.CategoryCode = drugCategory.CategoryCode;
            existing.CategoryName = drugCategory.CategoryName;
            existing.Description = drugCategory.Description;
            existing.IsActive = drugCategory.IsActive;
            existing.ModifiedBy = drugCategory.ModifiedBy;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDrugCategoryAsync(Guid id)
        {
            var drugCategory = await _dbContext.DrugCategories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (drugCategory == null)
            {
                return false;
            }

            // Soft delete / deactivate
            drugCategory.IsActive = false;
            drugCategory.ModifiedOn = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}