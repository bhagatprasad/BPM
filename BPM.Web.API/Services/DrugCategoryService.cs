using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DrugCategoryService : IDrugCategoryService
    {
        private readonly IDrugCategoryRepository _drugCategoryRepository;

        public DrugCategoryService(IDrugCategoryRepository drugCategoryRepository)
        {
            _drugCategoryRepository = drugCategoryRepository;
        }

        public async Task<List<DrugCategoryDto>> GetAllDrugCategoriesAsync()
        {
            var drugCategories = await _drugCategoryRepository.GetAllDrugCategoriesAsync();

            return drugCategories.ToDto();
        }

        public async Task<DrugCategoryDto?> GetDrugCategoryByIdAsync(Guid drugCategoryId)
        {
            var drugCategory = await _drugCategoryRepository.GetDrugCategoryByIdAsync(drugCategoryId);

            if (drugCategory == null)
                return null;

            return drugCategory.ToDto();
        }

        public async Task<bool> InsertDrugCategoryAsync(CreateDrugCategoryDto drugCategoryDto)
        {
            var drugCategory = drugCategoryDto.ToEntity();

            return await _drugCategoryRepository.InsertDrugCategoryAsync(drugCategory);
        }

        public async Task<bool> UpdateDrugCategoryAsync(UpdateDrugCategoryDto drugCategoryDto)
        {
            var drugCategory = drugCategoryDto.ToEntity();

            return await _drugCategoryRepository.UpdateDrugCategoryAsync(drugCategory);
        }

        public async Task<bool> DeleteDrugCategoryAsync(Guid drugCategoryId)
        {
            return await _drugCategoryRepository.DeleteDrugCategoryAsync(drugCategoryId);
        }
    }
}