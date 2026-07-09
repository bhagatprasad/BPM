using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using Microsoft.Extensions.Logging;

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
                var suppliers = await _repository.GetAllSuppliersAsync();
                return suppliers.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all suppliers");
                throw;
            }
        }

        public async Task<SupplierDto?> GetSupplierByIdAsync(Guid supplierId)
        {
            try
            {
                var supplier = await _repository.GetSupplierByIdAsync(supplierId);
                return supplier?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting supplier with ID: {SupplierId}", supplierId);
                throw;
            }
        }

        public async Task<bool> InsertSupplierAsync(CreateSupplierDto supplierDto)
        {
            try
            {
                // Validate if supplier code already exists
                if (await SupplierCodeExistsAsync(supplierDto.SupplierCode))
                {
                    throw new InvalidOperationException($"Supplier code '{supplierDto.SupplierCode}' already exists");
                }

                var supplier = supplierDto.ToEntity();
                var success = await _repository.InsertSupplierAsync(supplier);

                if (!success)
                {
                    throw new InvalidOperationException("Failed to insert supplier");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting supplier with code: {SupplierCode}", supplierDto.SupplierCode);
                throw;
            }
        }

        public async Task<bool> UpdateSupplierAsync(UpdateSupplierDto supplierDto)
        {
            try
            {
                // Get existing supplier
                var existingSupplier = await _repository.GetSupplierByIdAsync(supplierDto.Id);
                if (existingSupplier == null)
                {
                    _logger.LogWarning("Supplier with ID: {SupplierId} not found for update", supplierDto.Id);
                    return false;
                }

                // Check if supplier code is being changed and if new code already exists
                if (existingSupplier.SupplierCode != supplierDto.SupplierCode)
                {
                    if (await SupplierCodeExistsAsync(supplierDto.SupplierCode, supplierDto.Id))
                    {
                        _logger.LogWarning("Supplier code '{SupplierCode}' already exists", supplierDto.SupplierCode);
                        return false;
                    }
                }

                // Map DTO to existing entity
                supplierDto.MapToEntity(existingSupplier);
                return await _repository.UpdateSupplierAsync(existingSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating supplier with ID: {SupplierId}", supplierDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteSupplierAsync(Guid supplierId)
        {
            try
            {
                var supplier = await _repository.GetSupplierByIdAsync(supplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID: {SupplierId} not found for deletion", supplierId);
                    return false;
                }

                return await _repository.DeleteSupplierAsync(supplierId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting supplier with ID: {SupplierId}", supplierId);
                throw;
            }
        }

        // Additional methods for validation
        public async Task<SupplierDto?> GetSupplierByCodeAsync(string supplierCode)
        {
            try
            {
                var suppliers = await _repository.GetAllSuppliersAsync();
                var supplier = suppliers.FirstOrDefault(s => s.SupplierCode == supplierCode);
                return supplier?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting supplier by code: {SupplierCode}", supplierCode);
                throw;
            }
        }

        public async Task<List<SupplierDto>> GetActiveSuppliersAsync()
        {
            try
            {
                var suppliers = await _repository.GetAllSuppliersAsync();
                var activeSuppliers = suppliers.Where(s => s.IsActive).ToList();
                return activeSuppliers.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active suppliers");
                throw;
            }
        }

        public async Task<bool> ToggleSupplierStatusAsync(Guid supplierId, bool isActive)
        {
            try
            {
                var supplier = await _repository.GetSupplierByIdAsync(supplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID: {SupplierId} not found for status toggle", supplierId);
                    return false;
                }

                supplier.IsActive = isActive;
                supplier.ModifiedOn = DateTime.UtcNow;
                return await _repository.UpdateSupplierAsync(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling supplier status for ID: {SupplierId}", supplierId);
                throw;
            }
        }

        // Helper method to check if supplier code exists
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
