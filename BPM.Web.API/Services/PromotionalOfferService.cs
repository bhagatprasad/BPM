using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class PromotionalOfferService : IPromotionalOfferService
    {
        private readonly IPromotionalOfferRepository _repository;
        private readonly ILogger<PromotionalOfferService> _logger;

        public PromotionalOfferService(
            IPromotionalOfferRepository repository,
            ILogger<PromotionalOfferService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PromotionalOfferResponseDto> CreateAsync(PromotionalOfferCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating promotional offer for SupplierId: {SupplierId}", dto.SupplierId);

                if (dto.SupplierId == Guid.Empty)
                {
                    _logger.LogWarning("SupplierId is required.");
                    throw new ArgumentException("SupplierId is required.");
                }

                if (string.IsNullOrWhiteSpace(dto.OfferName))
                {
                    _logger.LogWarning("Offer name is required.");
                    throw new ArgumentException("Offer name is required.");
                }

                if (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 50)
                {
                    _logger.LogWarning("Invalid promotional discount percentage: {DiscountPercentage}", dto.DiscountPercentage);
                    throw new ArgumentException("Discount percentage must be between 0 and 50.");
                }

                if (dto.ExpiryDate < dto.StartDate)
                {
                    _logger.LogWarning("Invalid promotional offer dates.");
                    throw new ArgumentException("ExpiryDate cannot be earlier than StartDate.");
                }

                var entity = dto.ToEntity();

                var result = await _repository.CreateAsync(entity);

                _logger.LogInformation("Promotional offer created successfully. Id: {Id}", result.Id);

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating promotional offer.");
                throw;
            }
        }

        public async Task<IEnumerable<PromotionalOfferResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all promotional offers.");

                var result = await _repository.GetAllAsync();

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offers.");
                throw;
            }
        }

        public async Task<PromotionalOfferResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching promotional offer by Id: {Id}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid promotional offer Id.");
                    throw new ArgumentException("Invalid promotional offer Id.");
                }

                var result = await _repository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Promotional offer not found. Id: {Id}", id);
                    return null;
                }

                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offer. Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PromotionalOfferResponseDto>> GetBySupplierAsync(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Fetching promotional offers for SupplierId: {SupplierId}", supplierId);

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
                _logger.LogError(ex, "Error occurred while fetching promotional offers for SupplierId: {SupplierId}", supplierId);
                throw;
            }
        }

        public async Task<IEnumerable<PromotionalOfferResponseDto>> GetByDrugAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching promotional offers for DrugId: {DrugId}", drugId);

                if (drugId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid DrugId.");
                    throw new ArgumentException("Invalid DrugId.");
                }

                var result = await _repository.GetByDrugAsync(drugId);

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotional offers for DrugId: {DrugId}", drugId);
                throw;
            }
        }

        public async Task<IEnumerable<PromotionalOfferResponseDto>> GetActiveOffersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching active promotional offers.");

                var result = await _repository.GetActiveOffersAsync();

                return result.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active promotional offers.");
                throw;
            }
        }
    }
}
