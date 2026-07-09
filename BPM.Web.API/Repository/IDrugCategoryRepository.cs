using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDrugCategoryRepository
    {
        Task<List<DrugCategory>> GetAllDrugCategoriesAsync();

        Task<DrugCategory?> GetDrugCategoryByIdAsync(Guid drugCategoryId);

        Task<bool> InsertDrugCategoryAsync(DrugCategory drugCategory);

        Task<bool> UpdateDrugCategoryAsync(DrugCategory drugCategory);

        Task<bool> DeleteDrugCategoryAsync(Guid drugCategoryId);
    }
}
