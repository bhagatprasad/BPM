using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;

namespace BPM.Web.API.Services
{
    public class DrugUomService : IDrugUomService
    {
        private readonly IDrugUomRepository _repository;
        private readonly ILogger<DrugUomService> _logger;

        public DrugUomService(
            IDrugUomRepository repository,
            ILogger<DrugUomService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugUom>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all Drug UOMs");
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOMs");
                throw;
            }
        }

        public async Task<DrugUom?> GetByIdAsync(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug UOM with Id {UomId}", uomId);
                return await _repository.GetByIdAsync(uomId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOM with Id {UomId}", uomId);
                throw;
            }
        }

        public async Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug UOMs for Drug {DrugId}", drugId);
                return await _repository.GetByDrugIdAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOMs for Drug {DrugId}", drugId);
                throw;
            }
        }

        public async Task<DrugUom?> GetByCodeAsync(Guid drugId, string uomCode)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug UOM with Code {UomCode} for Drug {DrugId}", uomCode, drugId);
                return await _repository.GetByCodeAsync(drugId, uomCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug UOM with Code {UomCode}", uomCode);
                throw;
            }
        }

        public async Task<List<DrugUom>> GetBaseUnitsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving base units for Drug {DrugId}", drugId);
                return await _repository.GetBaseUnitsByDrugIdAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving base units for Drug {DrugId}", drugId);
                throw;
            }
        }

        public async Task<List<DrugUom>> GetPurchaseUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving purchase UOMs for Drug {DrugId}", drugId);
                return await _repository.GetPurchaseUomsByDrugIdAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase UOMs for Drug {DrugId}", drugId);
                throw;
            }
        }

        public async Task<List<DrugUom>> GetSalesUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving sales UOMs for Drug {DrugId}", drugId);
                return await _repository.GetSalesUomsByDrugIdAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sales UOMs for Drug {DrugId}", drugId);
                throw;
            }
        }

        public async Task<(bool Success, string Message)> InsertAsync(DrugUom drugUom)
        {
            try
            {
                _logger.LogInformation("Creating Drug UOM");

                // Validate UOM hierarchy
                if (!await ValidateUomHierarchyAsync(drugUom))
                {
                    return (false, "Invalid UOM hierarchy configuration.");
                }

                // Check for duplicate UOM code
                if (await _repository.ExistsAsync(drugUom.DrugId, drugUom.UomCode))
                {
                    return (false, $"UOM code '{drugUom.UomCode}' already exists for this drug.");
                }

                // Ensure at least one base unit exists
                if (drugUom.IsBaseUnit)
                {
                    var baseUnits = await _repository.GetBaseUnitsByDrugIdAsync(drugUom.DrugId);
                    if (baseUnits.Any())
                    {
                        return (false, "Only one base unit is allowed per drug.");
                    }
                }

                var result = await _repository.InsertAsync(drugUom);
                return result ? (true, "Drug UOM created successfully.") : (false, "Failed to create Drug UOM.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug UOM");
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(DrugUom drugUom)
        {
            try
            {
                _logger.LogInformation("Updating Drug UOM");

                // Validate UOM hierarchy
                if (!await ValidateUomHierarchyAsync(drugUom))
                {
                    return (false, "Invalid UOM hierarchy configuration.");
                }

                // Check for duplicate UOM code
                if (await _repository.ExistsAsync(drugUom.DrugId, drugUom.UomCode, drugUom.UomId))
                {
                    return (false, $"UOM code '{drugUom.UomCode}' already exists for this drug.");
                }

                // Prevent self-reference in hierarchy
                if (drugUom.ParentUomId.HasValue && drugUom.ParentUomId.Value == drugUom.UomId)
                {
                    return (false, "A UOM cannot be its own parent.");
                }

                // Ensure at least one base unit exists
                if (drugUom.IsBaseUnit)
                {
                    var baseUnits = await _repository.GetBaseUnitsByDrugIdAsync(drugUom.DrugId);
                    if (baseUnits.Any(x => x.UomId != drugUom.UomId))
                    {
                        return (false, "Only one base unit is allowed per drug.");
                    }
                }

                var result = await _repository.UpdateAsync(drugUom);
                return result ? (true, "Drug UOM updated successfully.") : (false, "Drug UOM not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug UOM");
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug UOM with Id {UomId}", uomId);

                // Check if UOM has child UOMs
                if (await _repository.HasChildUomsAsync(uomId))
                {
                    return (false, "Cannot delete UOM that has child UOMs. Please delete child UOMs first.");
                }

                var result = await _repository.DeleteAsync(uomId);
                return result ? (true, "Drug UOM deleted successfully.") : (false, "Drug UOM not found.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while deleting Drug UOM");
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug UOM with Id {UomId}", uomId);
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<bool> ValidateUomHierarchyAsync(DrugUom drugUom)
        {
            try
            {
                // If no parent UOM, it's valid (must be base unit or standalone)
                if (!drugUom.ParentUomId.HasValue)
                {
                    return true;
                }

                // Get parent UOM
                var parentUom = await _repository.GetByIdAsync(drugUom.ParentUomId.Value);
                if (parentUom == null)
                {
                    return false;
                }

                // Parent must be active
                if (!parentUom.IsActive)
                {
                    return false;
                }

                // Parent must belong to the same drug
                if (parentUom.DrugId != drugUom.DrugId)
                {
                    return false;
                }

                // Quantity per parent is required when parent is specified
                if (!drugUom.QuantityPerParent.HasValue || drugUom.QuantityPerParent.Value <= 0)
                {
                    return false;
                }

                // A UOM cannot be base unit if it has a parent
                if (drugUom.IsBaseUnit)
                {
                    return false;
                }

                // Prevent circular references
                return await CheckCircularReferenceAsync(drugUom.UomId, drugUom.ParentUomId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating UOM hierarchy");
                return false;
            }
        }

        private async Task<bool> CheckCircularReferenceAsync(Guid childId, Guid parentId, HashSet<Guid>? visited = null)
        {
            visited ??= new HashSet<Guid>();
            visited.Add(childId);

            if (visited.Contains(parentId))
                return false;

            var parent = await _repository.GetByIdAsync(parentId);
            if (parent == null)
                return false;

            if (!parent.ParentUomId.HasValue)
                return true;

            return await CheckCircularReferenceAsync(parentId, parent.ParentUomId.Value, visited);
        }
    }
}