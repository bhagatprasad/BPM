using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;

namespace BPM.Web.API.Services
{
    public class DrugUomService : IDrugUomService
    {
        private readonly IDrugUomRepository _repository;
        private readonly ILogger<DrugUomService> _logger;

        public DrugUomService(
            IDrugUomRepository repository,
            ILogger<DrugUomService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugUom>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all Drug UOMs");

                var data = await _repository.GetAllAsync();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOMs");
                throw;
            }
        }

        public async Task<DrugUom?> GetByIdAsync(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug UOM with Id {UomId}", uomId);

                var data = await _repository.GetByIdAsync(uomId);

                if (data == null)
                {
                    _logger.LogWarning("Drug UOM not found with Id {UomId}", uomId);
                    return null;
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOM");
                throw;
            }
        }

        public async Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug UOMs for Drug {DrugId}", drugId);

                return await _repository.GetByDrugIdAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOMs");
                throw;
            }
        }

        public async Task<bool> InsertAsync(DrugUom drugUom)
        {
            try
            {
                _logger.LogInformation("Creating Drug UOM");

                var result = await _repository.InsertAsync(drugUom);

                if (!result)
                {
                    _logger.LogWarning("Failed to create Drug UOM");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug UOM");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(DrugUom drugUom)
        {
            try
            {
                _logger.LogInformation("Updating Drug UOM");

                var result = await _repository.UpdateAsync(drugUom);

                if (!result)
                {
                    _logger.LogWarning("Drug UOM not found for update");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug UOM");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug UOM");

                var result = await _repository.DeleteAsync(uomId);

                if (!result)
                {
                    _logger.LogWarning("Drug UOM not found for deletion");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug UOM");
                throw;
            }
        }
    }
}