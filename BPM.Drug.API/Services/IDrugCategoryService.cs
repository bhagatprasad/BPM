using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Services
{
    public interface IDrugCategoryService
    {
        Task<List<DrugCategoryDto.ResponseDrugCategoryDto>> GetAllDrugCategoriesAsync();
        Task<DrugCategoryDto.ResponseDrugCategoryDto?> GetDrugCategoryByIdAsync(Guid id);
        Task<bool> CreateDrugCategoryAsync(DrugCategoryDto.CreateDrugCategoryDto dto);
        Task<bool> UpdateDrugCategoryAsync(DrugCategoryDto.UpdateDrugCategoryDto dto);
        Task<bool>DeleteDrugCategoryAsync(Guid id);
    }
}

