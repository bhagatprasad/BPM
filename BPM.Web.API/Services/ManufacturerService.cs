using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<ManufacturerService> _logger;

        public ManufacturerService(IManufacturerRepository manufacturerRepository, ILogger<ManufacturerService> logger)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
        }

        public async Task<List<ManufacturerDto>> GetAllManufacturersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all manufacturers");

                var manufacturers = await _manufacturerRepository.GetAllManufacturersAsync();

                return manufacturers.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving manufacturers");
                throw;
            }
        }

        public async Task<ManufacturerDto?> GetManufacturerByIdAsync(Guid manufacturerId)
        {
            try
            {
                _logger.LogInformation("Retrieving manufacturer with Id {ManufacturerId}", manufacturerId);

                var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId);

                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found with Id {ManufacturerId}", manufacturerId);
                    return null;
                }

                return manufacturer.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving manufacturer with Id {ManufacturerId}", manufacturerId);
                throw;
            }
        }

        public async Task<bool> InsertManufacturerAsync(CreateManufacturerDto manufacturerDto)
        {
            try
            {
                _logger.LogInformation("Creating manufacturer");

                var manufacturer = manufacturerDto.ToEntity();

                var result = await _manufacturerRepository.InsertManufacturerAsync(manufacturer);

                if (!result)
                {
                    _logger.LogWarning("Failed to create manufacturer");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating manufacturer");
                throw;
            }
        }

        public async Task<bool> UpdateManufacturerAsync(UpdateManufacturerDto manufacturerDto)
        {
            try
            {
                _logger.LogInformation("Updating manufacturer");

                var manufacturer = manufacturerDto.ToEntity();

                var result = await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);

                if (!result)
                {
                    _logger.LogWarning("Manufacturer not found for update");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating manufacturer");
                throw;
            }
        }

        public async Task<bool> DeleteManufacturerAsync(Guid manufacturerId)
        {
            try
            {
                _logger.LogInformation("Deleting manufacturer with Id {ManufacturerId}", manufacturerId);

                var result = await _manufacturerRepository.DeleteManufacturerAsync(manufacturerId);

                if (!result)
                {
                    _logger.LogWarning("Manufacturer not found for deletion with Id {ManufacturerId}", manufacturerId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting manufacturer with Id {ManufacturerId}", manufacturerId);
                throw;
            }
        }
    }
}