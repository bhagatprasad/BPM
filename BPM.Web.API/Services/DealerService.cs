using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DealerService : IDealerService
    {
        private readonly IDealerRepository _dealerRepository;
        private readonly ILogger<DealerService> _logger;

        public DealerService(IDealerRepository dealerRepository, ILogger<DealerService> logger)
        {
            _dealerRepository = dealerRepository;
            _logger = logger;
        }

        public async Task<List<DealerDto>> GetAllDealersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all dealers");

                var dealers = await _dealerRepository.GetAllDealersAsync();

                return dealers.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all dealers");
                throw;
            }
        }

        public async Task<DealerDto?> GetDealerByIdAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Retrieving dealer with Id {DealerId}", dealerId);

                var dealer = await _dealerRepository.GetDealerByIdAsync(dealerId);

                if (dealer == null)
                {
                    _logger.LogWarning("Dealer not found with Id {DealerId}", dealerId);
                    return null;
                }

                return dealer.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving dealer with Id {DealerId}", dealerId);
                throw;
            }
        }

        public async Task<bool> InsertDealerAsync(CreateDealerDto dealerDto)
        {
            try
            {
                _logger.LogInformation("Creating dealer");

                var dealer = dealerDto.ToEntity();

                var result = await _dealerRepository.InsertDealerAsync(dealer);

                if (!result)
                {
                    _logger.LogWarning("Failed to create dealer");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting dealer");
                throw;
            }
        }

        public async Task<bool> UpdateDealerAsync(UpdateDealerDto dealerDto)
        {
            try
            {
                _logger.LogInformation("Updating dealer");

                var existingDealer = await _dealerRepository.GetDealerByIdAsync(dealerDto.Id);

                if (existingDealer == null)
                {
                    _logger.LogWarning("Dealer not found with Id {DealerId}", dealerDto.Id);
                    return false;
                }

                dealerDto.MapToEntity(existingDealer);

                var result = await _dealerRepository.UpdateDealerAsync(existingDealer);

                if (!result)
                {
                    _logger.LogWarning("Failed to update dealer");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating dealer with Id {DealerId}", dealerDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteDealerAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Deleting dealer with Id {DealerId}", dealerId);

                var result = await _dealerRepository.DeleteDealerAsync(dealerId);

                if (!result)
                {
                    _logger.LogWarning("Dealer not found for deletion with Id {DealerId}", dealerId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting dealer with Id {DealerId}", dealerId);
                throw;
            }
        }
    }
}