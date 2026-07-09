using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class DrugCategoryRepository : IDrugCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DrugCategory>> GetAllDrugCategoriesAsync()
        {
            return await _context.DrugCategories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<DrugCategory?> GetDrugCategoryByIdAsync(Guid drugCategoryId)
        {
            return await _context.DrugCategories
                .FirstOrDefaultAsync(x => x.Id == drugCategoryId);
        }

        public async Task<bool> InsertDrugCategoryAsync(DrugCategory drugCategory)
        {
            drugCategory.CreatedOn = DateTime.UtcNow;
            drugCategory.IsActive = true;

            _context.DrugCategories.Add(drugCategory);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDrugCategoryAsync(DrugCategory drugCategory)
        {
            var existingDrugCategory = await _context.DrugCategories.FindAsync(drugCategory.Id);

            if (existingDrugCategory == null)
                return false;

            existingDrugCategory.CategoryCode = drugCategory.CategoryCode;
            existingDrugCategory.CategoryName = drugCategory.CategoryName;
            existingDrugCategory.Description = drugCategory.Description;
            existingDrugCategory.IsActive = drugCategory.IsActive;
            existingDrugCategory.ModifiedBy = drugCategory.ModifiedBy;
            existingDrugCategory.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDrugCategoryAsync(Guid drugCategoryId)
        {
            var drugCategory = await _context.DrugCategories.FindAsync(drugCategoryId);

            if (drugCategory == null)
                return false;

            _context.DrugCategories.Remove(drugCategory);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}