using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DrugFormService : IDrugFormService
    {
        private readonly IDrugFormRepository _repository;
        private readonly ILogger<DrugFormService> _logger;

        // Common form types for validation
        private static readonly HashSet<string> ValidFormTypes = new()
        {
            "SOLID", "LIQUID", "SEMISOLID", "GAS", "POWDER", "GRANULE",
            "CAPSULE", "TABLET", "INJECTION", "SYRUP", "SUSPENSION",
            "EMULSION", "OINTMENT", "CREAM", "GEL", "PATCH", "SPRAY",
            "INHALER", "DROPS", "SUPPOSITORY", "IMPLANT"
        };

        public DrugFormService(
            IDrugFormRepository repository,
            ILogger<DrugFormService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugFormDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all Drug Forms");
                var data = await _repository.GetAllAsync();
                return data.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Forms");
                throw;
            }
        }

        public async Task<DrugFormDto?> GetByIdAsync(Guid formId)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Form with Id {FormId}", formId);
                var data = await _repository.GetByIdAsync(formId);
                return data?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Form with Id {FormId}", formId);
                throw;
            }
        }

        public async Task<DrugFormDto?> GetByCodeAsync(string formCode)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Form with Code {FormCode}", formCode);
                var data = await _repository.GetByCodeAsync(formCode);
                return data?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Form with Code {FormCode}", formCode);
                throw;
            }
        }

        public async Task<DrugFormDto?> GetByNameAsync(string formName)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Form with Name {FormName}", formName);
                var data = await _repository.GetByNameAsync(formName);
                return data?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Form with Name {FormName}", formName);
                throw;
            }
        }

        public async Task<List<DrugFormDto>> GetByTypeAsync(string formType)
        {
            try
            {
                _logger.LogInformation("Retrieving Drug Forms with Type {FormType}", formType);
                var data = await _repository.GetByTypeAsync(formType);
                return data.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Drug Forms with Type {FormType}", formType);
                throw;
            }
        }

        public async Task<List<DrugFormDto>> GetActiveFormsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active Drug Forms");
                var data = await _repository.GetActiveFormsAsync();
                return data.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving active Drug Forms");
                throw;
            }
        }

        public async Task<(List<DrugFormDto> Items, int TotalCount)> GetFilteredAsync(DrugFormFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Retrieving filtered Drug Forms");
                var (items, totalCount) = await _repository.GetFilteredAsync(filter);
                return (items.ToDtoList(), totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving filtered Drug Forms");
                throw;
            }
        }

        public async Task<(bool Success, string Message, DrugFormDto? Data)> CreateAsync(CreateDrugFormDto dto)
        {
            try
            {
                _logger.LogInformation("Creating Drug Form");

                // Trim and uppercase
                dto.FormCode = dto.FormCode.ToUpper().Trim();
                dto.FormName = dto.FormName.Trim();
                dto.FormType = dto.FormType?.Trim();

                // Validate
                var validationResult = await ValidateCreateAsync(dto);
                if (!validationResult.Success)
                    return (false, validationResult.Message, null);

                var entity = dto.ToEntity();
                var result = await _repository.InsertAsync(entity);

                if (!result)
                    return (false, "Failed to create Drug Form.", null);

                var created = await _repository.GetByIdAsync(entity.FormId);
                return (true, "Drug Form created successfully.", created?.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug Form");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, List<DrugFormDto> Data)> CreateBulkAsync(List<CreateDrugFormDto> dtos)
        {
            try
            {
                _logger.LogInformation("Creating multiple Drug Forms");

                var entities = new List<DrugForm>();
                var validationErrors = new List<string>();

                foreach (var dto in dtos)
                {
                    // Trim and uppercase
                    dto.FormCode = dto.FormCode.ToUpper().Trim();
                    dto.FormName = dto.FormName.Trim();
                    dto.FormType = dto.FormType?.Trim();

                    var validationResult = await ValidateCreateAsync(dto);
                    if (!validationResult.Success)
                    {
                        validationErrors.Add($"{dto.FormCode}: {validationResult.Message}");
                        continue;
                    }

                    entities.Add(dto.ToEntity());
                }

                if (validationErrors.Any())
                {
                    return (false, $"Validation errors: {string.Join("; ", validationErrors)}", new List<DrugFormDto>());
                }

                if (!entities.Any())
                {
                    return (false, "No valid forms to create.", new List<DrugFormDto>());
                }

                var result = await _repository.InsertBulkAsync(entities);
                if (!result)
                    return (false, "Failed to create Drug Forms.", new List<DrugFormDto>());

                var created = await _repository.GetAllAsync();
                var createdDtos = created
                    .Where(x => entities.Select(e => e.FormCode).Contains(x.FormCode))
                    .ToDtoList();

                return (true, $"{entities.Count} Drug Forms created successfully.", createdDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating multiple Drug Forms");
                return (false, $"Error: {ex.Message}", new List<DrugFormDto>());
            }
        }

        public async Task<(bool Success, string Message, DrugFormDto? Data)> UpdateAsync(UpdateDrugFormDto dto)
        {
            try
            {
                _logger.LogInformation("Updating Drug Form with Id {FormId}", dto.FormId);

                var existing = await _repository.GetByIdAsync(dto.FormId);
                if (existing == null)
                    return (false, "Drug Form not found.", null);

                // Trim and uppercase
                dto.FormCode = dto.FormCode.ToUpper().Trim();
                dto.FormName = dto.FormName.Trim();
                dto.FormType = dto.FormType?.Trim();

                // Validate
                var validationResult = await ValidateUpdateAsync(dto);
                if (!validationResult.Success)
                    return (false, validationResult.Message, null);

                var entity = dto.ToEntity();
                var result = await _repository.UpdateAsync(entity);

                if (!result)
                    return (false, "Failed to update Drug Form.", null);

                var updated = await _repository.GetByIdAsync(dto.FormId);
                return (true, "Drug Form updated successfully.", updated?.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug Form with Id {FormId}", dto.FormId);
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Guid formId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug Form with Id {FormId}", formId);

                var existing = await _repository.GetByIdAsync(formId);
                if (existing == null)
                    return (false, "Drug Form not found.");

                var result = await _repository.DeleteAsync(formId);
                return result ? (true, "Drug Form deleted successfully.") : (false, "Failed to delete Drug Form.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cannot delete form with associated drugs");
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug Form with Id {FormId}", formId);
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> SoftDeleteAsync(Guid formId)
        {
            try
            {
                _logger.LogInformation("Soft deleting Drug Form with Id {FormId}", formId);

                var existing = await _repository.GetByIdAsync(formId);
                if (existing == null)
                    return (false, "Drug Form not found.");

                var result = await _repository.SoftDeleteAsync(formId);
                return result ? (true, "Drug Form deactivated successfully.") : (false, "Failed to deactivate Drug Form.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while soft deleting Drug Form with Id {FormId}", formId);
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ActivateAsync(Guid formId)
        {
            try
            {
                _logger.LogInformation("Activating Drug Form with Id {FormId}", formId);

                var existing = await _repository.GetByIdAsync(formId);
                if (existing == null)
                    return (false, "Drug Form not found.");

                existing.IsActive = true;
                existing.ModifiedOn = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(existing);
                return result ? (true, "Drug Form activated successfully.") : (false, "Failed to activate Drug Form.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while activating Drug Form with Id {FormId}", formId);
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds)
        {
            try
            {
                _logger.LogInformation("Getting drug counts for {Count} forms", formIds.Count);
                return await _repository.GetDrugCountsByFormAsync(formIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting drug counts by forms");
                throw;
            }
        }

        public async Task<List<string>> GetFormTypesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all form types");
                var forms = await _repository.GetAllAsync();
                return forms
                    .Where(x => !string.IsNullOrEmpty(x.FormType))
                    .Select(x => x.FormType!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving form types");
                throw;
            }
        }

        public async Task<bool> ValidateFormCodeAsync(string formCode, Guid? excludeId = null)
        {
            return !await _repository.ExistsByCodeAsync(formCode, excludeId);
        }

        public async Task<bool> ValidateFormNameAsync(string formName, Guid? excludeId = null)
        {
            return !await _repository.ExistsByNameAsync(formName, excludeId);
        }

        private async Task<(bool Success, string Message)> ValidateCreateAsync(CreateDrugFormDto dto)
        {
            // Validate form code
            if (string.IsNullOrWhiteSpace(dto.FormCode))
                return (false, "Form code is required.");

            if (dto.FormCode.Length > 20)
                return (false, "Form code cannot exceed 20 characters.");

            // Validate form name
            if (string.IsNullOrWhiteSpace(dto.FormName))
                return (false, "Form name is required.");

            if (dto.FormName.Length > 100)
                return (false, "Form name cannot exceed 100 characters.");

            // Check for duplicates
            if (await _repository.ExistsByCodeAsync(dto.FormCode))
                return (false, $"Form code '{dto.FormCode}' already exists.");

            if (await _repository.ExistsByNameAsync(dto.FormName))
                return (false, $"Form name '{dto.FormName}' already exists.");

            // Validate form type if provided
            if (!string.IsNullOrEmpty(dto.FormType))
            {
                if (dto.FormType.Length > 50)
                    return (false, "Form type cannot exceed 50 characters.");

                // Optional: Check against valid form types
                if (!ValidFormTypes.Contains(dto.FormType.ToUpper()))
                {
                    _logger.LogWarning("Form type '{FormType}' is not in the standard list", dto.FormType);
                    // Allow but log warning
                }
            }

            return (true, "Validation successful.");
        }

        private async Task<(bool Success, string Message)> ValidateUpdateAsync(UpdateDrugFormDto dto)
        {
            // Validate form code
            if (string.IsNullOrWhiteSpace(dto.FormCode))
                return (false, "Form code is required.");

            if (dto.FormCode.Length > 20)
                return (false, "Form code cannot exceed 20 characters.");

            // Validate form name
            if (string.IsNullOrWhiteSpace(dto.FormName))
                return (false, "Form name is required.");

            if (dto.FormName.Length > 100)
                return (false, "Form name cannot exceed 100 characters.");

            // Check for duplicates (excluding current)
            if (await _repository.ExistsByCodeAsync(dto.FormCode, dto.FormId))
                return (false, $"Form code '{dto.FormCode}' already exists.");

            if (await _repository.ExistsByNameAsync(dto.FormName, dto.FormId))
                return (false, $"Form name '{dto.FormName}' already exists.");

            // Validate form type if provided
            if (!string.IsNullOrEmpty(dto.FormType) && dto.FormType.Length > 50)
                return (false, "Form type cannot exceed 50 characters.");

            return (true, "Validation successful.");
        }
    }
}
