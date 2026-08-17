
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IDistributorRepository _distributorRepository;
        private readonly ILogger<DistributorService> _logger;
        public DistributorService(IDistributorRepository distributorRepository, ILogger<DistributorService> logger)
        {
            _distributorRepository = distributorRepository;
            _logger = logger;
        }

        public async Task<DistributorDto> GetDistributorByIdAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching distributor by distributorId");
                var distributor = await _distributorRepository.GetDistributorByIdAsync(distributorId);

                if (distributor == null)
                {
                    _logger.LogWarning("Distributor not found with Id {DistributorId}", distributorId);
                    return null;
                }
                return distributor.ToDto();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while retrieving distributor with Id {DistributorId}", distributorId);
                throw;
            }
        }

        public async Task<List<DistributorDto>> GetDistributorListAsync()
        {
            try
            {
                _logger.LogInformation("fetching list of distributors");
                var distributors = await _distributorRepository.GetAllDistributorsAsync();
                return distributors.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("unable to fetch distributors");
                throw;
            }
        }

        public async Task<bool> InsertDistributorAsync(CreateDistributorDto distributorDto)
        {
            try
            {
                _logger.LogInformation("Creating Distributor");
                var distributor = distributorDto.ToEntity();
                var result = await _distributorRepository.InsertDistributorAsync(distributor);
                if (!result)
                {
                    _logger.LogError("Failed to create Distributor");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating distributor: {Message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                }
                throw;
            }
        }



        public async Task<DistributorDto?> UpdateDistributorAsync(Guid distributorId, UpdateDistributorDto updateDistributor)
        {
            try
            {
                _logger.LogInformation("Starting update for distributor with Id: {DistributorId}", distributorId);

                var dbDistributor = await _distributorRepository.GetDistributorByIdAsync(distributorId);
                if (dbDistributor == null)
                {
                    _logger.LogWarning("Distributor not found with Id: {DistributorId}", distributorId);
                    return null;
                }

                var updatedDbDistributor = updateDistributor.ToEntity(dbDistributor);
                var updateResult = await _distributorRepository.UpdateDistributorAsync(updatedDbDistributor);
                if (!updateResult)
                {
                    _logger.LogError("Failed to update distributor with Id: {DistributorId}", distributorId);
                    return null;
                }

                _logger.LogInformation("Distributor updated successfully with Id: {DistributorId}", distributorId);
                return updatedDbDistributor.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating distributor with Id: {DistributorId}", distributorId);
                throw;
            }
        }
        public async Task<bool> DeleteDistributorById(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Deleting distributor with Id: {DistributorId}", distributorId);

                var result = await _distributorRepository.DeleteDistributorAsync(distributorId);
                if (!result)
                {
                    _logger.LogError("Failed to delete distributor with Id: {DistributorId}", distributorId);
                    return false;
                }

                _logger.LogInformation("Distributor deleted successfully. Id: {DistributorId}", distributorId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting distributor with Id: {DistributorId}", distributorId);
                throw;
            }
        }
    }
}

