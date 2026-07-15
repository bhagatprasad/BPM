using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;

namespace BPM.Web.API.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _repository;
        private readonly ILogger<DrugService> _logger;

        public DrugService(IDrugRepository repository, ILogger<DrugService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<Drug>> GetAllDrugsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all drugs");

                var drugs = await _repository.GetAllDrugsAsync();

                return drugs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drugs");
                throw;
            }
        }

        public async Task<Drug?> GetDrugByIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug with Id {DrugId}", drugId);

                var drug = await _repository.GetDrugByIdAsync(drugId);

                if (drug == null)
                {
                    _logger.LogWarning("Drug not found with Id {DrugId}", drugId);
                    return null;
                }

                return drug;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug with Id {DrugId}", drugId);
                throw;
            }
        }

        public async Task<bool> InsertDrugAsync(Drug drug)
        {
            try
            {
                _logger.LogInformation("Creating drug");

                var result = await _repository.InsertDrugAsync(drug);

                if (!result)
                {
                    _logger.LogWarning("Failed to create drug");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting drug");
                throw;
            }
        }

        public async Task<bool> UpdateDrugAsync(Drug drug)
        {
            try
            {
                _logger.LogInformation("Updating drug");

                var result = await _repository.UpdateDrugAsync(drug);

                if (!result)
                {
                    _logger.LogWarning("Drug not found for update");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug with Id {DrugId}", drug.DrugId);
                throw;
            }
        }

        public async Task<bool> DeleteDrugAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Deleting drug with Id {DrugId}", drugId);

                var result = await _repository.DeleteDrugAsync(drugId);

                if (!result)
                {
                    _logger.LogWarning("Drug not found for deletion with Id {DrugId}", drugId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug with Id {DrugId}", drugId);
                throw;
            }
        }
    }
}