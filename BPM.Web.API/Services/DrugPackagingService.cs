using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DrugPackagingService : IDrugPackagingService
    {
        private readonly IDrugPackagingRepository _repository;
        private readonly IDrugUomRepository _uomRepository;
        private readonly IDrugRepository _drugRepository;
        private readonly ILogger<DrugPackagingService> _logger;

        public DrugPackagingService(
            IDrugPackagingRepository repository,
            IDrugUomRepository uomRepository,
            IDrugRepository drugRepository,
            ILogger<DrugPackagingService> logger)
        {
            _repository = repository;
            _uomRepository = uomRepository;
            _drugRepository = drugRepository;
            _logger = logger;
        }

        public async Task<List<DrugPackagingDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all Drug Packaging");
                var data = await _repository.GetAllAsync();
                return data.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Packaging");
                throw;
            }
        }

        public async Task<DrugPackagingDto?> GetByIdAsync(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Packaging with Id {PackagingId}", packagingId);
                var data = await _repository.GetByIdAsync(packagingId);
                return data?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Packaging with Id {PackagingId}", packagingId);
                throw;
            }
        }

        public async Task<List<DrugPackagingDto>> GetByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Packaging for Drug {DrugId}", drugId);
                var data = await _repository.GetByDrugIdAsync(drugId);
                return data.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Packaging for Drug {DrugId}", drugId);
                throw;
            }
        }

        public async Task<DrugPackagingDto?> GetByBarcodeAsync(string barcode)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Packaging with Barcode {Barcode}", barcode);
                var data = await _repository.GetByBarcodeAsync(barcode);
                return data?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Packaging with Barcode {Barcode}", barcode);
                throw;
            }
        }

        public async Task<(List<DrugPackagingDto> Items, int TotalCount)> GetFilteredAsync(DrugPackagingFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Retrieving filtered Drug Packaging");
                var (items, totalCount) = await _repository.GetFilteredAsync(filter);
                return (items.ToDtoList(), totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving filtered Drug Packaging");
                throw;
            }
        }

        public async Task<(bool Success, string Message, DrugPackagingDto? Data)> CreateAsync(CreateDrugPackagingDto dto)
        {
            try
            {
                _logger.LogInformation("Creating Drug Packaging");

                // Validate
                var validationResult = await ValidatePackagingAsync(dto);
                if (!validationResult.Success)
                {
                    return (false, validationResult.Message, null);
                }

                // Check for duplicate barcode
                if (!string.IsNullOrEmpty(dto.Barcode))
                {
                    if (await _repository.ExistsByBarcodeAsync(dto.Barcode))
                    {
                        return (false, $"Barcode '{dto.Barcode}' already exists.", null);
                    }
                }

                // Validate UOM compatibility
                var uomValid = await _repository.ValidateUomCompatibilityAsync(dto.PackageUomId, dto.ContainsUomId);
                if (!uomValid)
                {
                    return (false, "The package UOM and contains UOM are not compatible. Package UOM must contain multiple units of the contains UOM.", null);
                }

                var entity = dto.ToEntity();
                var result = await _repository.InsertAsync(entity);

                if (!result)
                    return (false, "Failed to create Drug Packaging.", null);

                var created = await _repository.GetByIdAsync(entity.PackagingId);
                return (true, "Drug Packaging created successfully.", created?.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug Packaging");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, DrugPackagingDto? Data)> UpdateAsync(UpdateDrugPackagingDto dto)
        {
            try
            {
                _logger.LogInformation("Updating Drug Packaging with Id {PackagingId}", dto.PackagingId);

                var existing = await _repository.GetByIdAsync(dto.PackagingId);
                if (existing == null)
                    return (false, "Drug Packaging not found.", null);

                // Validate
                var validationResult = await ValidatePackagingAsync(dto);
                if (!validationResult.Success)
                {
                    return (false, validationResult.Message, null);
                }

                // Check for duplicate barcode
                if (!string.IsNullOrEmpty(dto.Barcode))
                {
                    if (await _repository.ExistsByBarcodeAsync(dto.Barcode, dto.PackagingId))
                    {
                        return (false, $"Barcode '{dto.Barcode}' already exists.", null);
                    }
                }

                // Validate UOM compatibility
                var uomValid = await _repository.ValidateUomCompatibilityAsync(dto.PackageUomId, dto.ContainsUomId);
                if (!uomValid)
                {
                    return (false, "The package UOM and contains UOM are not compatible. Package UOM must contain multiple units of the contains UOM.", null);
                }

                var entity = dto.ToEntity();
                var result = await _repository.UpdateAsync(entity);

                if (!result)
                    return (false, "Failed to update Drug Packaging.", null);

                var updated = await _repository.GetByIdAsync(dto.PackagingId);
                return (true, "Drug Packaging updated successfully.", updated?.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug Packaging with Id {PackagingId}", dto.PackagingId);
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug Packaging with Id {PackagingId}", packagingId);

                var existing = await _repository.GetByIdAsync(packagingId);
                if (existing == null)
                    return (false, "Drug Packaging not found.");

                var result = await _repository.DeleteAsync(packagingId);
                return result ? (true, "Drug Packaging deleted successfully.") : (false, "Failed to delete Drug Packaging.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug Packaging with Id {PackagingId}", packagingId);
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ValidatePackagingAsync(CreateDrugPackagingDto dto)
        {
            // Validate drug exists
            var drug = await _drugRepository.GetDrugByIdAsync(dto.DrugId);
            if (drug == null || !drug.IsActive)
                return (false, "Drug not found or inactive.");

            // Validate package UOM exists and belongs to the drug
            var packageUom = await _uomRepository.GetByIdAsync(dto.PackageUomId);
            if (packageUom == null || !packageUom.IsActive)
                return (false, "Package UOM not found or inactive.");

            if (packageUom.DrugId != dto.DrugId)
                return (false, "Package UOM does not belong to the specified drug.");

            // Validate contains UOM exists and belongs to the drug
            var containsUom = await _uomRepository.GetByIdAsync(dto.ContainsUomId);
            if (containsUom == null || !containsUom.IsActive)
                return (false, "Contains UOM not found or inactive.");

            if (containsUom.DrugId != dto.DrugId)
                return (false, "Contains UOM does not belong to the specified drug.");

            // Validate quantity
            if (dto.Quantity < 1)
                return (false, "Quantity must be at least 1.");

            // Validate total units
            if (dto.TotalUnits < dto.Quantity)
                return (false, "Total units must be at least the quantity.");

            // Validate prices
            if (dto.UnitPrice <= 0)
                return (false, "Unit price must be greater than zero.");

            if (dto.PackagePrice <= 0)
                return (false, "Package price must be greater than zero.");

            // Validate package price matches unit price * total units (with tolerance)
            var expectedPackagePrice = dto.UnitPrice * dto.TotalUnits;
            var tolerance = 0.01m; // 1 cent tolerance
            if (Math.Abs(dto.PackagePrice - expectedPackagePrice) > tolerance)
            {
                return (false, $"Package price ({dto.PackagePrice:C}) does not match unit price ({dto.UnitPrice:C}) × total units ({dto.TotalUnits}) = {expectedPackagePrice:C}");
            }

            return (true, "Validation successful.");
        }

        public async Task<(bool Success, string Message)> ValidatePackagingAsync(UpdateDrugPackagingDto dto)
        {
            // Similar validation for update DTO
            return await ValidatePackagingAsync(new CreateDrugPackagingDto
            {
                DrugId = dto.DrugId,
                PackageUomId = dto.PackageUomId,
                ContainsUomId = dto.ContainsUomId,
                Quantity = dto.Quantity,
                TotalUnits = dto.TotalUnits,
                UnitPrice = dto.UnitPrice,
                PackagePrice = dto.PackagePrice
            });
        }

        public async Task<decimal> CalculatePackagePriceAsync(Guid packageUomId, Guid containsUomId, int quantity, decimal unitPrice)
        {
            // Get UOMs
            var packageUom = await _uomRepository.GetByIdAsync(packageUomId);
            var containsUom = await _uomRepository.GetByIdAsync(containsUomId);

            if (packageUom == null || containsUom == null)
                throw new ArgumentException("Invalid UOM provided.");

            // Calculate total units based on conversion factor
            var totalUnits = quantity * (int)(packageUom.ConversionFactor / containsUom.ConversionFactor);
            return unitPrice * totalUnits;
        }

        public async Task<Dictionary<Guid, decimal>> GetPackagingSummaryByDrugAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Getting packaging summary for Drug {DrugId}", drugId);

                var packagings = await _repository.GetByDrugIdAsync(drugId);
                var summary = new Dictionary<Guid, decimal>();

                foreach (var packaging in packagings)
                {
                    var totalUnits = packaging.TotalUnits * packaging.Quantity;
                    summary[packaging.PackagingId] = totalUnits;
                }

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting packaging summary for Drug {DrugId}", drugId);
                throw;
            }
        }
    }
}