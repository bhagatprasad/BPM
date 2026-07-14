using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DrugCategoryService : IDrugCategoryService
    {
        private readonly IDrugCategoryRepository _drugCategoryRepository;
        private readonly ILogger<DrugCategoryService> _logger;

        public DrugCategoryService(IDrugCategoryRepository drugCategoryRepository, ILogger<DrugCategoryService> logger)
        {
            _drugCategoryRepository = drugCategoryRepository;
            _logger = logger;
        }

        public async Task<List<DrugCategoryDto>> GetAllDrugCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all drug categories");

                var drugCategories = await _drugCategoryRepository.GetAllDrugCategoriesAsync();

                return drugCategories.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drug categories");
                throw;
            }
        }

        public async Task<DrugCategoryDto?> GetDrugCategoryByIdAsync(Guid drugCategoryId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug category with Id {DrugCategoryId}", drugCategoryId);

                var drugCategory = await _drugCategoryRepository.GetDrugCategoryByIdAsync(drugCategoryId);

                if (drugCategory == null)
                {
                    _logger.LogWarning("Drug category not found with Id {DrugCategoryId}", drugCategoryId);
                    return null;
                }

                return drugCategory.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug category with Id {DrugCategoryId}", drugCategoryId);
                throw;
            }
        }

        public async Task<bool> InsertDrugCategoryAsync(CreateDrugCategoryDto drugCategoryDto)
        {
            try
            {
                _logger.LogInformation("Creating drug category");

                var drugCategory = drugCategoryDto.ToEntity();

                var result = await _drugCategoryRepository.InsertDrugCategoryAsync(drugCategory);

                if (!result)
                {
                    _logger.LogWarning("Failed to create drug category");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting drug category");
                throw;
            }
        }

        public async Task<bool> UpdateDrugCategoryAsync(UpdateDrugCategoryDto drugCategoryDto)
        {
            try
            {
                _logger.LogInformation("Updating drug category");

                var existingDrugCategory = await _drugCategoryRepository.GetDrugCategoryByIdAsync(drugCategoryDto.Id);

                if (existingDrugCategory == null)
                {
                    _logger.LogWarning("Drug category not found with Id {DrugCategoryId}", drugCategoryDto.Id);
                    return false;
                }

                drugCategoryDto.MapToEntity(existingDrugCategory);

                var result = await _drugCategoryRepository.UpdateDrugCategoryAsync(existingDrugCategory);

                if (!result)
                {
                    _logger.LogWarning("Failed to update drug category");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug category with Id {DrugCategoryId}", drugCategoryDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteDrugCategoryAsync(Guid drugCategoryId)
        {
            try
            {
                _logger.LogInformation("Deleting drug category with Id {DrugCategoryId}", drugCategoryId);

                var result = await _drugCategoryRepository.DeleteDrugCategoryAsync(drugCategoryId);

                if (!result)
                {
                    _logger.LogWarning("Drug category not found for deletion with Id {DrugCategoryId}", drugCategoryId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug category with Id {DrugCategoryId}", drugCategoryId);
                throw;
            }
        }
    }
}