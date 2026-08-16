using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly IDiscountCodeRepository _repository;
        private readonly ILogger<DiscountCodeService> _logger;

        public DiscountCodeService(
            IDiscountCodeRepository repository,
            ILogger<DiscountCodeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DiscountCodeResponseDto> CreateAsync(DiscountCodeCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating discount code: {DiscountCode}", dto.DiscountCode);

                if (string.IsNullOrWhiteSpace(dto.DiscountCode))
                {
                    _logger.LogWarning("Discount code is required.");
                    throw new ArgumentException("Discount code is required.");
                }

                if (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 50)
                {
                    _logger.LogWarning("Invalid discount percentage: {DiscountPercentage}", dto.DiscountPercentage);
                    throw new ArgumentException("Discount percentage must be between 0 and 50.");
                }

                if (dto.ExpiryDate < dto.StartDate)
                {
                    _logger.LogWarning("Invalid discount code dates.");
                    throw new ArgumentException("ExpiryDate cannot be earlier than StartDate.");
                }

                var existingCode = await _repository.GetByCodeAsync(dto.DiscountCode);

                if (existingCode != null)
                {
                    _logger.LogWarning("Discount code already exists: {DiscountCode}", dto.DiscountCode);
                    throw new ArgumentException("Discount code already exists.");
                }

                var entity = dto.ToEntity();

                var result = await _repository.CreateAsync(entity);

                _logger.LogInformation("Discount code created successfully. Id: {Id}", result.Id);

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating discount code.");
                throw;
            }
        }

        public async Task<IEnumerable<DiscountCodeResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all discount codes.");

                var result = await _repository.GetAllAsync();

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching discount codes.");
                throw;
            }
        }

        public async Task<DiscountCodeResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching discount code by Id: {Id}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid discount code Id.");
                    throw new ArgumentException("Invalid discount code Id.");
                }

                var result = await _repository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Discount code not found. Id: {Id}", id);
                    return null;
                }

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching discount code. Id: {Id}", id);
                throw;
            }
        }

        public async Task<DiscountCodeResponseDto?> GetByCodeAsync(string discountCode)
        {
            try
            {
                _logger.LogInformation("Fetching discount code: {DiscountCode}", discountCode);

                if (string.IsNullOrWhiteSpace(discountCode))
                {
                    _logger.LogWarning("Discount code is required.");
                    throw new ArgumentException("Discount code is required.");
                }

                var result = await _repository.GetByCodeAsync(discountCode);

                if (result == null)
                {
                    _logger.LogWarning("Discount code not found: {DiscountCode}", discountCode);
                    return null;
                }

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching discount code: {DiscountCode}", discountCode);
                throw;
            }
        }

        public async Task<IEnumerable<DiscountCodeResponseDto>> GetBySupplierAsync(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Fetching discount codes for SupplierId: {SupplierId}", supplierId);

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
                _logger.LogError(ex, "Error occurred while fetching discount codes for SupplierId: {SupplierId}", supplierId);
                throw;
            }
        }

        public async Task<IEnumerable<DiscountCodeResponseDto>> GetActiveCodesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching active discount codes.");

                var result = await _repository.GetActiveCodesAsync();

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active discount codes.");
                throw;
            }
        }
    }
}