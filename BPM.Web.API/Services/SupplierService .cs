using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(ISupplierRepository repository, ILogger<SupplierService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<SupplierDto>> GetAllSuppliersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all suppliers");

                var suppliers = await _repository.GetAllSuppliersAsync();

                return suppliers.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all suppliers");
                throw;
            }
        }

        public async Task<SupplierDto?> GetSupplierByIdAsync(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Retrieving supplier with Id {SupplierId}", supplierId);

                var supplier = await _repository.GetSupplierByIdAsync(supplierId);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier not found with Id {SupplierId}", supplierId);
                    return null;
                }

                return supplier.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier with Id {SupplierId}", supplierId);
                throw;
            }
        }

        public async Task<bool> InsertSupplierAsync(CreateSupplierDto supplierDto)
        {
            try
            {
                _logger.LogInformation("Creating supplier");

                if (await SupplierCodeExistsAsync(supplierDto.SupplierCode))
                {
                    _logger.LogWarning("Supplier code {SupplierCode} already exists", supplierDto.SupplierCode);
                    return false;
                }

                var supplier = supplierDto.ToEntity();

                var result = await _repository.InsertSupplierAsync(supplier);

                if (!result)
                {
                    _logger.LogWarning("Failed to create supplier");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier");
                throw;
            }
        }

        public async Task<bool> UpdateSupplierAsync(UpdateSupplierDto supplierDto)
        {
            try
            {
                _logger.LogInformation("Updating supplier");

                var existingSupplier = await _repository.GetSupplierByIdAsync(supplierDto.Id);

                if (existingSupplier == null)
                {
                    _logger.LogWarning("Supplier not found with Id {SupplierId}", supplierDto.Id);
                    return false;
                }

                if (existingSupplier.SupplierCode != supplierDto.SupplierCode)
                {
                    if (await SupplierCodeExistsAsync(supplierDto.SupplierCode, supplierDto.Id))
                    {
                        _logger.LogWarning("Supplier code {SupplierCode} already exists", supplierDto.SupplierCode);
                        return false;
                    }
                }

                supplierDto.MapToEntity(existingSupplier);

                var result = await _repository.UpdateSupplierAsync(existingSupplier);

                if (!result)
                {
                    _logger.LogWarning("Failed to update supplier");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating supplier with Id {SupplierId}", supplierDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteSupplierAsync(Guid supplierId)
        {
            try
            {
                _logger.LogInformation("Deleting supplier with Id {SupplierId}", supplierId);

                var supplier = await _repository.GetSupplierByIdAsync(supplierId);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier not found with Id {SupplierId}", supplierId);
                    return false;
                }

                var result = await _repository.DeleteSupplierAsync(supplierId);

                if (!result)
                {
                    _logger.LogWarning("Failed to delete supplier");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting supplier with Id {SupplierId}", supplierId);
                throw;
            }
        }

        public async Task<SupplierDto?> GetSupplierByCodeAsync(string supplierCode)
        {
            try
            {
                _logger.LogInformation("Retrieving supplier with Code {SupplierCode}", supplierCode);

                var suppliers = await _repository.GetAllSuppliersAsync();
                var supplier = suppliers.FirstOrDefault(s => s.SupplierCode == supplierCode);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier not found with Code {SupplierCode}", supplierCode);
                    return null;
                }

                return supplier.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier with Code {SupplierCode}", supplierCode);
                throw;
            }
        }

        public async Task<List<SupplierDto>> GetActiveSuppliersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active suppliers");

                var suppliers = await _repository.GetAllSuppliersAsync();
                var activeSuppliers = suppliers.Where(s => s.IsActive).ToList();

                return activeSuppliers.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving active suppliers");
                throw;
            }
        }

        public async Task<bool> ToggleSupplierStatusAsync(Guid supplierId, bool isActive)
        {
            try
            {
                _logger.LogInformation("Updating supplier status");

                var supplier = await _repository.GetSupplierByIdAsync(supplierId);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier not found with Id {SupplierId}", supplierId);
                    return false;
                }

                supplier.IsActive = isActive;
                supplier.ModifiedOn = DateTime.UtcNow;

                var result = await _repository.UpdateSupplierAsync(supplier);

                if (!result)
                {
                    _logger.LogWarning("Failed to update supplier status");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating supplier status with Id {SupplierId}", supplierId);
                throw;
            }
        }

        private async Task<bool> SupplierCodeExistsAsync(string supplierCode, Guid? excludeId = null)
        {
            var allSuppliers = await _repository.GetAllSuppliersAsync();

            if (excludeId.HasValue)
            {
                return allSuppliers.Any(s => s.SupplierCode == supplierCode && s.SupplierId != excludeId.Value);
            }

            return allSuppliers.Any(s => s.SupplierCode == supplierCode);
        }
    }
}