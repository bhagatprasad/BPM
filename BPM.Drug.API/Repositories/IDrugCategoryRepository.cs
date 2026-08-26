using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Repositories
{
    public interface IDrugCategoryRepository
    {
        Task<List<DrugCategory>> GetAllDrugCategoriesAsync();
        Task<DrugCategory?> GetDrugCategoryByIdAsync(Guid id);
        Task<bool>InsertDrugCategoryAsync(DrugCategory drugCategory);
        Task<bool> UpdateDrugCategoryAsync(DrugCategory drugCategory);
        Task<bool> DeleteDrugCategoryAsync(Guid id);
    }
}


