using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;
using BPM.Web.Drug.API.Models.Mappers;
using BPM.Web.Drug.API.Repositories.Interfaces;
using BPM.Web.Drug.API.Services.Interfaces;

namespace BPM.Web.Drug.API.Services
{
    public class DrugFormService : IDrugFormService
    {
        private readonly IDrugFormRepository _repository;
        private readonly ILogger<DrugFormService> _logger;

        private static readonly HashSet<string> ValidFormTypes = new()
        {
            "SOLID",
            "LIQUID",
            "SEMISOLID",
            "GAS",
            "POWDER",
            "GRANULE",
            "CAPSULE",
            "TABLET",
            "INJECTION",
            "SYRUP",
            "SUSPENSION",
            "EMULSION",
            "OINTMENT",
            "CREAM",
            "GEL",
            "PATCH",
            "SPRAY",
            "INHALER",
            "DROPS",
            "SUPPOSITORY",
            "IMPLANT"
        };

        public DrugFormService(IDrugFormRepository repository, ILogger<DrugFormService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugFormDto.ResponseDrugFormDto>> GetAllDrugFormsAsync()
        {
            _logger.LogInformation("Retrieving all Drug Forms.");
            var drugForms = await _repository.GetAllDrugFormsAsync();
            return drugForms.ToDtoList();
        }

        public async Task<DrugFormDto.ResponseDrugFormDto?> GetDrugFormByIdAsync(Guid formId)
        {
            _logger.LogInformation("Retrieving Drug Form by Id {FormId}.", formId);
            var drugForm = await _repository.GetDrugFormByIdAsync(formId);
            return drugForm?.ToDto();
        }

        public async Task<DrugFormDto.ResponseDrugFormDto?> GetDrugFormByCodeAsync(string formCode)
        {
            _logger.LogInformation("Retrieving Drug Form by Code {FormCode}.", formCode);
            var drugForm = await _repository.GetDrugFormByCodeAsync(formCode);
            return drugForm?.ToDto();
        }

        public async Task<DrugFormDto.ResponseDrugFormDto?> GetDrugFormByNameAsync(string formName)
        {
            _logger.LogInformation("Retrieving Drug Form by Name {FormName}.", formName);
            var drugForm = await _repository.GetDrugFormByNameAsync(formName);
            return drugForm?.ToDto();
        }

        public async Task<List<DrugFormDto.ResponseDrugFormDto>> GetDrugFormsByTypeAsync(string formType)
        {
            _logger.LogInformation("Retrieving Drug Forms by Type {FormType}.", formType);
            var drugForms = await _repository.GetDrugFormsByTypeAsync(formType);
            return drugForms.ToDtoList();
        }

        public async Task<List<DrugFormDto.ResponseDrugFormDto>> GetActiveDrugFormsAsync()
        {
            _logger.LogInformation("Retrieving active Drug Forms.");
            var drugForms = await _repository.GetActiveDrugFormsAsync();
            return drugForms.ToDtoList();
        }

        public async Task<(List<DrugFormDto.ResponseDrugFormDto> Items, int TotalCount)> GetFilteredDrugFormsAsync(DrugFormDto.DrugFormFilterDto filter)
        {
            _logger.LogInformation("Retrieving filtered Drug Forms.");
            var result = await _repository.GetFilteredDrugFormsAsync(filter);
            return (result.Items.ToDtoList(), result.TotalCount);
        }

        public async Task<(bool Success, string Message, DrugFormDto.ResponseDrugFormDto? Data)> CreateDrugFormAsync(DrugFormDto.CreateDrugFormDto dto)
        {
            try
            {
                _logger.LogInformation("Creating Drug Form with Code {FormCode}.", dto.FormCode);
                dto.FormCode = dto.FormCode.ToUpper().Trim();
                dto.FormName = dto.FormName.Trim();
                dto.FormType = dto.FormType?.Trim();

                if (string.IsNullOrWhiteSpace(dto.FormCode))
                {
                    return (false, "Form code is required.", null);
                }

                if (dto.FormCode.Length > 20)
                {
                    return (false, "Form code cannot exceed 20 characters.", null);
                }

                if (string.IsNullOrWhiteSpace(dto.FormName))
                {
                    return (false, "Form name is required.", null);
                }

                if (dto.FormName.Length > 100)
                {
                    return (false, "Form name cannot exceed 100 characters.", null);
                }

                if (await _repository.DrugFormCodeExistsAsync(dto.FormCode))
                {
                    return (false, $"Form code '{dto.FormCode}' already exists.", null);
                }

                if (await _repository.DrugFormNameExistsAsync(dto.FormName))
                {
                    return (false, $"Form name '{dto.FormName}' already exists.", null);
                }

                if (!string.IsNullOrWhiteSpace(dto.FormType))
                {
                    if (dto.FormType.Length > 50)
                    {
                        return (false, "Form type cannot exceed 50 characters.", null);
                    }

                    if (!ValidFormTypes.Contains(dto.FormType.ToUpper()))
                    {
                        _logger.LogWarning("Form type {FormType} is not in the standard list.", dto.FormType);
                    }
                }

                var drugForm = dto.ToEntity();
                var result = await _repository.InsertDrugFormAsync(drugForm);

                if (!result)
                {
                    return (false, "Failed to create Drug Form.", null);
                }

                var createdDrugForm = await _repository.GetDrugFormByIdAsync(drugForm.FormId);
                return (true, "Drug Form created successfully.", createdDrugForm?.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug Form.");
                return (false, "An error occurred while creating Drug Form.", null);
            }
        }

        public async Task<(bool Success, string Message, List<DrugFormDto.ResponseDrugFormDto> Data)> CreateBulkDrugFormsAsync(List<DrugFormDto.CreateDrugFormDto> dtos)
        {
            try
            {
                _logger.LogInformation("Creating {Count} Drug Forms in bulk.", dtos.Count);
                var drugForms = new List<DrugFormEntity>();

                foreach (var dto in dtos)
                {
                    dto.FormCode = dto.FormCode.ToUpper().Trim();
                    dto.FormName = dto.FormName.Trim();
                    dto.FormType = dto.FormType?.Trim();

                    if (string.IsNullOrWhiteSpace(dto.FormCode))
                    {
                        return (false, "Form code is required.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    if (dto.FormCode.Length > 20)
                    {
                        return (false, "Form code cannot exceed 20 characters.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    if (string.IsNullOrWhiteSpace(dto.FormName))
                    {
                        return (false, "Form name is required.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    if (dto.FormName.Length > 100)
                    {
                        return (false, "Form name cannot exceed 100 characters.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    if (await _repository.DrugFormCodeExistsAsync(dto.FormCode))
                    {
                        return (false, $"Form code '{dto.FormCode}' already exists.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    if (await _repository.DrugFormNameExistsAsync(dto.FormName))
                    {
                        return (false, $"Form name '{dto.FormName}' already exists.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    if (!string.IsNullOrWhiteSpace(dto.FormType) && dto.FormType.Length > 50)
                    {
                        return (false, "Form type cannot exceed 50 characters.", new List<DrugFormDto.ResponseDrugFormDto>());
                    }

                    drugForms.Add(dto.ToEntity());
                }

                var result = await _repository.InsertBulkDrugFormsAsync(drugForms);

                if (!result)
                {
                    return (false, "Failed to create Drug Forms.", new List<DrugFormDto.ResponseDrugFormDto>());
                }

                var response = drugForms.ToDtoList();
                return (true, $"{drugForms.Count} Drug Forms created successfully.", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug Forms in bulk.");
                return (false, "An error occurred while creating Drug Forms.", new List<DrugFormDto.ResponseDrugFormDto>());
            }
        }

        public async Task<(bool Success, string Message, DrugFormDto.ResponseDrugFormDto? Data)> UpdateDrugFormAsync(DrugFormDto.UpdateDrugFormDto dto)
        {
            try
            {
                _logger.LogInformation("Updating Drug Form with Id {FormId}.", dto.FormId);
                var existingDrugForm = await _repository.GetDrugFormByIdAsync(dto.FormId);

                if (existingDrugForm == null)
                {
                    return (false, "Drug Form not found.", null);
                }

                dto.FormCode = dto.FormCode.ToUpper().Trim();
                dto.FormName = dto.FormName.Trim();
                dto.FormType = dto.FormType?.Trim();

                if (string.IsNullOrWhiteSpace(dto.FormCode))
                {
                    return (false, "Form code is required.", null);
                }

                if (dto.FormCode.Length > 20)
                {
                    return (false, "Form code cannot exceed 20 characters.", null);
                }

                if (string.IsNullOrWhiteSpace(dto.FormName))
                {
                    return (false, "Form name is required.", null);
                }

                if (dto.FormName.Length > 100)
                {
                    return (false, "Form name cannot exceed 100 characters.", null);
                }

                if (await _repository.DrugFormCodeExistsAsync(dto.FormCode, dto.FormId))
                {
                    return (false, $"Form code '{dto.FormCode}' already exists.", null);
                }

                if (await _repository.DrugFormNameExistsAsync(dto.FormName, dto.FormId))
                {
                    return (false, $"Form name '{dto.FormName}' already exists.", null);
                }

                if (!string.IsNullOrWhiteSpace(dto.FormType) && dto.FormType.Length > 50)
                {
                    return (false, "Form type cannot exceed 50 characters.", null);
                }

                var drugForm = dto.ToEntity();
                var result = await _repository.UpdateDrugFormAsync(drugForm);

                if (!result)
                {
                    return (false, "Failed to update Drug Form.", null);
                }

                var updatedDrugForm = await _repository.GetDrugFormByIdAsync(dto.FormId);
                return (true, "Drug Form updated successfully.", updatedDrugForm?.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug Form with Id {FormId}.", dto.FormId);
                return (false, "An error occurred while updating Drug Form.", null);
            }
        }

        public async Task<(bool Success, string Message)> SoftDeleteDrugFormAsync(Guid formId)
        {
            try
            {
                _logger.LogInformation("Deactivating Drug Form with Id {FormId}.", formId);
                var existingDrugForm = await _repository.GetDrugFormByIdAsync(formId);

                if (existingDrugForm == null)
                {
                    return (false, "Drug Form not found.");
                }

                var result = await _repository.SoftDeleteDrugFormAsync(formId);
                return result ? (true, "Drug Form deactivated successfully.") : (false, "Failed to deactivate Drug Form.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deactivating Drug Form with Id {FormId}.", formId);
                return (false, "An error occurred while deactivating Drug Form.");
            }
        }

        public async Task<(bool Success, string Message)> ActivateDrugFormAsync(Guid formId)
        {
            try
            {
                _logger.LogInformation("Activating Drug Form with Id {FormId}.", formId);
                var existingDrugForm = await _repository.GetDrugFormByIdAsync(formId);

                if (existingDrugForm == null)
                {
                    return (false, "Drug Form not found.");
                }

                existingDrugForm.IsActive = true;
                existingDrugForm.ModifiedOn = DateTime.UtcNow;

                var drugForm = new DrugFormEntity
                {
                    FormId = existingDrugForm.FormId,
                    FormCode = existingDrugForm.FormCode,
                    FormName = existingDrugForm.FormName,
                    FormType = existingDrugForm.FormType,
                    IsActive = existingDrugForm.IsActive,
                    CreatedOn = existingDrugForm.CreatedOn,
                    ModifiedOn = existingDrugForm.ModifiedOn,
                    ModifiedBy = existingDrugForm.ModifiedBy
                };

                var result = await _repository.UpdateDrugFormAsync(drugForm);
                return result ? (true, "Drug Form activated successfully.") : (false, "Failed to activate Drug Form.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while activating Drug Form with Id {FormId}.", formId);
                return (false, "An error occurred while activating Drug Form.");
            }
        }

        public async Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds)
        {
            _logger.LogInformation("Retrieving drug counts for Drug Forms.");
            return await _repository.GetDrugCountsByFormAsync(formIds);
        }

        public async Task<List<string>> GetDrugFormTypesAsync()
        {
            _logger.LogInformation("Retrieving Drug Form Types.");
            var drugForms = await _repository.GetAllDrugFormsAsync();
            return drugForms.Where(x => !string.IsNullOrWhiteSpace(x.FormType)).Select(x => x.FormType!).Distinct().OrderBy(x => x).ToList();
        }

        public async Task<bool> ValidateDrugFormCodeAsync(string formCode, Guid? excludeId = null)
        {
            return !await _repository.DrugFormCodeExistsAsync(formCode, excludeId);
        }

        public async Task<bool> ValidateDrugFormNameAsync(string formName, Guid? excludeId = null)
        {
            return !await _repository.DrugFormNameExistsAsync(formName, excludeId);
        }
    }
}