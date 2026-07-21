using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;

namespace BPM.Web.API.Services
{
    public class PackagingMasterService : IPackagingMasterService
    {
        private readonly IPackagingMasterRepository _repository;
        private readonly ILogger<PackagingMasterService> _logger;

        public PackagingMasterService(
            IPackagingMasterRepository repository,
            ILogger<PackagingMasterService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<PackagingMaster>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all Packaging.");

                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving Packaging.");
                throw;
            }
        }

        public async Task<PackagingMaster?> GetByIdAsync(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Retrieving Packaging {PackagingId}", packagingId);

                var data = await _repository.GetByIdAsync(packagingId);

                if (data == null)
                {
                    _logger.LogWarning("Packaging not found.");
                    return null;
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Packaging.");
                throw;
            }
        }

        public async Task<bool> InsertAsync(PackagingMaster packaging)
        {
            try
            {
                _logger.LogInformation("Creating Packaging.");

                return await _repository.InsertAsync(packaging);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Packaging.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(PackagingMaster packaging)
        {
            try
            {
                _logger.LogInformation("Updating Packaging.");

                return await _repository.UpdateAsync(packaging);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Packaging.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Deleting Packaging.");

                return await _repository.DeleteAsync(packagingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Packaging.");
                throw;
            }
        }
    }
}