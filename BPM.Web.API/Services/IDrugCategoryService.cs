using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IDrugCategoryService
    {
        Task<List<DrugCategoryDto>> GetAllDrugCategoriesAsync();

        Task<DrugCategoryDto?> GetDrugCategoryByIdAsync(Guid drugCategoryId);

        Task<bool> InsertDrugCategoryAsync(CreateDrugCategoryDto drugCategoryDto);

        Task<bool> UpdateDrugCategoryAsync(UpdateDrugCategoryDto drugCategoryDto);

        Task<bool> DeleteDrugCategoryAsync(Guid drugCategoryId);
    }
}