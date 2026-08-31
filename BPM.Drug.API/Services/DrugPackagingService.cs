using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Mappers;
using BPM.Web.Drug.API.Repositories;

namespace BPM.Web.Drug.API.Services
{
    public class DrugPackagingService : IDrugPackagingService
    {
        private readonly IDrugPackagingRepository _repository;
        private readonly ILogger<DrugPackagingService> _logger;

        public DrugPackagingService(IDrugPackagingRepository repository, ILogger<DrugPackagingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all drug packagings");

                var packagings = await _repository.GetAllAsync();

                return packagings.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drug packagings");

                throw;
            }
        }

        public async Task<DrugPackagingDto.ResponseDrugPackagingDto?> GetByIdAsync(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug packaging with Id {PackagingId}", packagingId);

                var packaging = await _repository.GetByIdAsync(packagingId);

                if (packaging == null)
                {
                    _logger.LogWarning("Drug packaging not found with Id {PackagingId}", packagingId);

                    return null;
                }

                return packaging.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packaging with Id {PackagingId}", packagingId);

                throw;
            }
        }

        public async Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug packagings for DrugId {DrugId}", drugId);

                var packagings = await _repository.GetByDrugIdAsync(drugId);

                return packagings.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings for DrugId {DrugId}", drugId);

                throw;
            }
        }

        public async Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByPackageUomIdAsync(Guid packageUomId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug packagings for PackageUomId {PackageUomId}", packageUomId);

                var packagings = await _repository.GetByPackageUomIdAsync(packageUomId);

                return packagings.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings for PackageUomId {PackageUomId}", packageUomId);

                throw;
            }
        }

        public async Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByContainsUomIdAsync(Guid containsUomId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug packagings for ContainsUomId {ContainsUomId}", containsUomId);

                var packagings = await _repository.GetByContainsUomIdAsync(containsUomId);

                return packagings.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings for ContainsUomId {ContainsUomId}", containsUomId);

                throw;
            }
        }

        public async Task<DrugPackagingDto.ResponseDrugPackagingDto?> GetByBarcodeAsync(string barcode)
        {
            try
            {
                _logger.LogInformation("Retrieving drug packaging with Barcode {Barcode}", barcode);

                var packaging = await _repository.GetByBarcodeAsync(barcode);

                if (packaging == null)
                {
                    _logger.LogWarning("Drug packaging not found with Barcode {Barcode}", barcode);

                    return null;
                }

                return packaging.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packaging with Barcode {Barcode}", barcode);

                throw;
            }
        }

        public async Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            try
            {
                _logger.LogInformation("Retrieving drug packagings between prices {MinPrice} and {MaxPrice}", minPrice, maxPrice);

                var packagings = await _repository.GetByPriceRangeAsync(minPrice, maxPrice);

                return packagings.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug packagings by price range");

                throw;
            }
        }

        public async Task<(List<DrugPackagingDto.ResponseDrugPackagingDto> Items, int TotalCount)> GetFilteredAsync(DrugPackagingDto.DrugPackagingFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Retrieving filtered drug packagings");

                var result = await _repository.GetFilteredAsync(filter);

                var items = result.Items.ToDtoList();

                return (items, result.TotalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving filtered drug packagings");

                throw;
            }
        }

        public async Task<bool> CreateAsync(DrugPackagingDto.CreateDrugPackagingDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug packaging");

                if (!string.IsNullOrWhiteSpace(dto.Barcode))
                {
                    var barcodeExists = await _repository.ExistsByBarcodeAsync(dto.Barcode);

                    if (barcodeExists)
                    {
                        _logger.LogWarning("Drug packaging already exists with Barcode {Barcode}", dto.Barcode);

                        return false;
                    }
                }

                var isCompatible = await _repository.ValidateUomCompatibilityAsync(dto.PackageUomId, dto.ContainsUomId);

                if (!isCompatible)
                {
                    _logger.LogWarning("Invalid UOM compatibility between PackageUomId {PackageUomId} and ContainsUomId {ContainsUomId}", dto.PackageUomId, dto.ContainsUomId);

                    return false;
                }

                var packaging = dto.ToEntity();

                return await _repository.InsertAsync(packaging);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug packaging");

                throw;
            }
        }

        public async Task<bool> UpdateAsync(DrugPackagingDto.UpdateDrugPackagingDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug packaging with Id {PackagingId}", dto.PackagingId);

                var existing = await _repository.GetByIdAsync(dto.PackagingId);

                if (existing == null)
                {
                    _logger.LogWarning("Drug packaging not found with Id {PackagingId}", dto.PackagingId);
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(dto.Barcode))
                {
                    var barcodeExists = await _repository.ExistsByBarcodeAsync(dto.Barcode, dto.PackagingId);

                    if (barcodeExists)
                    {
                        _logger.LogWarning("Another drug packaging already exists with Barcode {Barcode}", dto.Barcode);
                        return false;
                    }
                }

                var isCompatible = await _repository.ValidateUomCompatibilityAsync(dto.PackageUomId, dto.ContainsUomId);

                if (!isCompatible)
                {
                    _logger.LogWarning("Invalid UOM compatibility between PackageUomId {PackageUomId} and ContainsUomId {ContainsUomId}", dto.PackageUomId, dto.ContainsUomId);
                    return false;
                }

                existing.DrugId = dto.DrugId;
                existing.PackageUomId = dto.PackageUomId;
                existing.ContainsUomId = dto.ContainsUomId;
                existing.Quantity = dto.Quantity;
                existing.TotalUnits = dto.TotalUnits;
                existing.UnitPrice = dto.UnitPrice;
                existing.PackagePrice = dto.PackagePrice;
                existing.Barcode = dto.Barcode;
                existing.GrossWeight = dto.GrossWeight;
                existing.NetWeight = dto.NetWeight;
                existing.Length = dto.Length;
                existing.Width = dto.Width;
                existing.Height = dto.Height;
                existing.IsActive = dto.IsActive;

                return await _repository.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug packaging with Id {PackagingId}", dto.PackagingId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid packagingId)
        {
            try
            {
                _logger.LogInformation("Deleting drug packaging with Id {PackagingId}", packagingId);

                var existing = await _repository.GetByIdAsync(packagingId);

                if (existing == null)
                {
                    _logger.LogWarning("Drug packaging not found with Id {PackagingId}", packagingId);

                    return false;
                }

                return await _repository.DeleteAsync(packagingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug packaging with Id {PackagingId}", packagingId);

                throw;
            }
        }

        public async Task<decimal> GetTotalPackagesByDrugAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving total packages for DrugId {DrugId}", drugId);

                return await _repository.GetTotalPackagesByDrugAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving total packages for DrugId {DrugId}", drugId);

                throw;
            }
        }
    }
}
