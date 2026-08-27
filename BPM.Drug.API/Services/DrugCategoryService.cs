using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Mappers;
using BPM.Web.Drug.API.Repositories;

namespace BPM.Web.Drug.API.Services
{
    public class DrugCategoryService : IDrugCategoryService
    {
        private readonly IDrugCategoryRepository _repository;
        private readonly ILogger<DrugCategoryService> _logger;

        public DrugCategoryService(IDrugCategoryRepository repository, ILogger<DrugCategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugCategoryDto.ResponseDrugCategoryDto>> GetAllDrugCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all drug categories");
                var categories = await _repository.GetAllDrugCategoriesAsync();
                return categories.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drug categories");
                throw;
            }
        }

        public async Task<DrugCategoryDto.ResponseDrugCategoryDto?> GetDrugCategoryByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving drug category with Id {CategoryId}", id);
                var category = await _repository.GetDrugCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Drug category not found with Id {CategoryId}", id);
                    return null;
                }
                return category.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug category with Id {CategoryId}", id);
                throw;
            }
        }

        public async Task<bool> CreateDrugCategoryAsync(DrugCategoryDto.CreateDrugCategoryDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug category with Code {CategoryCode}", dto.CategoryCode);
                var category = dto.ToEntity();
                return await _repository.InsertDrugCategoryAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug category");
                throw;
            }
        }

        public async Task<bool> UpdateDrugCategoryAsync(DrugCategoryDto.UpdateDrugCategoryDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug category with Id {CategoryId}", dto.Id);
                var category = dto.ToEntity();
                return await _repository.UpdateDrugCategoryAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug category with Id {CategoryId}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteDrugCategoryAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting drug category with Id {CategoryId}", id);
                return await _repository.DeleteDrugCategoryAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug category with Id {CategoryId}", id);
                throw;
            }
        }
    }
}