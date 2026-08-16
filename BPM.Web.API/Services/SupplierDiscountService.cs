using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;


namespace BPM.Web.API.Services
{
    public class SupplierDiscountService : ISupplierDiscountService
    {
        private readonly ISupplierDiscountRepository _repository;
        private readonly ILogger<SupplierDiscountService> _logger;

        public SupplierDiscountService(
            ISupplierDiscountRepository repository,
            ILogger<SupplierDiscountService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SupplierDiscountResponseDto> CreateAsync(SupplierDiscountCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating supplier discount for SupplierId: {SupplierId}", dto.SupplierId);

                if (dto.SupplierId == Guid.Empty)
                {
                    _logger.LogWarning("SupplierId is required.");
                    throw new ArgumentException("SupplierId is required.");
                }

                if (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 50)
                {
                    _logger.LogWarning("Invalid supplier discount percentage: {DiscountPercentage}", dto.DiscountPercentage);
                    throw new ArgumentException("Discount percentage must be between 0 and 50.");
                }

                if (dto.ValidTo.HasValue && dto.ValidTo < dto.ValidFrom)
                {
                    _logger.LogWarning("Invalid supplier discount validity dates.");
                    throw new ArgumentException("ValidTo cannot be earlier than ValidFrom.");
                }

                var entity = dto.ToEntity();

                var result = await _repository.CreateAsync(entity);

                _logger.LogInformation("Supplier discount created successfully. Id: {Id}", result.Id);

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier discount.");
                throw;
            }
        }

        public async Task<IEnumerable<SupplierDiscountResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all supplier discounts.");

                var result = await _repository.GetAllAsync();

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier discounts.");
                throw;
            }
        }

        public async Task<SupplierDiscountResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching supplier discount by Id: {Id}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid supplier discount Id.");
                    throw new ArgumentException("Invalid supplier discount Id.");
                }

                var result = await _repository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Supplier discount not found. Id: {Id}", id);
                    return null;
                }

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier discount. Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<SupplierDiscountResponseDto>> GetBySupplierAsync(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Fetching supplier discounts for SupplierId: {SupplierId}", supplierId);

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
                _logger.LogError(ex, "Error occurred while fetching supplier discounts for SupplierId: {SupplierId}", supplierId);
                throw;
            }
        }
    }
}
