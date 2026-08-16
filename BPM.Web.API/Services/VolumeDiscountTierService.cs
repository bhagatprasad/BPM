using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class VolumeDiscountTierService : IVolumeDiscountTierService
    {
        private readonly IVolumeDiscountTierRepository _repository;
        private readonly ILogger<VolumeDiscountTierService> _logger;

        public VolumeDiscountTierService(
            IVolumeDiscountTierRepository repository,
            ILogger<VolumeDiscountTierService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<VolumeDiscountTierResponseDto> CreateAsync(VolumeDiscountTierCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating volume discount tier for SupplierId: {SupplierId}", dto.SupplierId);

                if (dto.SupplierId == Guid.Empty)
                {
                    _logger.LogWarning("SupplierId is required.");
                    throw new ArgumentException("SupplierId is required.");
                }

                if (dto.MinQuantity <= 0)
                {
                    _logger.LogWarning("Minimum quantity must be greater than zero.");
                    throw new ArgumentException("Minimum quantity must be greater than zero.");
                }

                if (dto.MaxQuantity.HasValue && dto.MaxQuantity < dto.MinQuantity)
                {
                    _logger.LogWarning("Invalid volume discount quantity range.");
                    throw new ArgumentException("MaxQuantity cannot be less than MinQuantity.");
                }

                if (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 50)
                {
                    _logger.LogWarning("Invalid discount percentage: {DiscountPercentage}", dto.DiscountPercentage);
                    throw new ArgumentException("Discount percentage must be between 0 and 50.");
                }

                var entity = dto.ToEntity();

                var result = await _repository.CreateAsync(entity);

                _logger.LogInformation("Volume discount tier created successfully. Id: {Id}", result.Id);

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating volume discount tier.");
                throw;
            }
        }

        public async Task<IEnumerable<VolumeDiscountTierResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all volume discount tiers.");

                var result = await _repository.GetAllAsync();

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching volume discount tiers.");
                throw;
            }
        }

        public async Task<VolumeDiscountTierResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching volume discount tier by Id: {Id}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid volume discount tier Id.");
                    throw new ArgumentException("Invalid volume discount tier Id.");
                }

                var result = await _repository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Volume discount tier not found. Id: {Id}", id);
                    return null;
                }

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching volume discount tier. Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<VolumeDiscountTierResponseDto>> GetBySupplierAsync(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Fetching volume discount tiers for SupplierId: {SupplierId}", supplierId);

                if (supplierId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid SupplierId.");
                    throw new ArgumentException("Invalid SupplierId.");
                }

                var result = await _repository.GetBySupplierAsync(supplierId);

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching volume discount tiers for SupplierId: {SupplierId}", supplierId);
                throw;
            }
        }
    }
}
