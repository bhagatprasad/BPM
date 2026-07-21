using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IDrugFormService
    {
        Task<List<DrugFormDto>> GetAllAsync();
        Task<DrugFormDto?> GetByIdAsync(Guid formId);
        Task<DrugFormDto?> GetByCodeAsync(string formCode);
        Task<DrugFormDto?> GetByNameAsync(string formName);
        Task<List<DrugFormDto>> GetByTypeAsync(string formType);
        Task<List<DrugFormDto>> GetActiveFormsAsync();
        Task<(List<DrugFormDto> Items, int TotalCount)> GetFilteredAsync(DrugFormFilterDto filter);
        Task<(bool Success, string Message, DrugFormDto? Data)> CreateAsync(CreateDrugFormDto dto);
        Task<(bool Success, string Message, List<DrugFormDto> Data)> CreateBulkAsync(List<CreateDrugFormDto> dtos);
        Task<(bool Success, string Message, DrugFormDto? Data)> UpdateAsync(UpdateDrugFormDto dto);
        Task<(bool Success, string Message)> DeleteAsync(Guid formId);
        Task<(bool Success, string Message)> SoftDeleteAsync(Guid formId);
        Task<(bool Success, string Message)> ActivateAsync(Guid formId);
        Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds);
        Task<List<string>> GetFormTypesAsync();
        Task<bool> ValidateFormCodeAsync(string formCode, Guid? excludeId = null);
        Task<bool> ValidateFormNameAsync(string formName, Guid? excludeId = null);
    }
}
