using BPM.Web.Drug.API.Models.DTOs;

namespace BPM.Web.Drug.API.Services.Interfaces
{
    public interface IDrugFormService
    {
        Task<List<DrugFormDto.ResponseDrugFormDto>> GetAllDrugFormsAsync();

        Task<DrugFormDto.ResponseDrugFormDto?> GetDrugFormByIdAsync(Guid formId);

        Task<DrugFormDto.ResponseDrugFormDto?> GetDrugFormByCodeAsync(string formCode);

        Task<DrugFormDto.ResponseDrugFormDto?> GetDrugFormByNameAsync(string formName);

        Task<List<DrugFormDto.ResponseDrugFormDto>> GetDrugFormsByTypeAsync(string formType);

        Task<List<DrugFormDto.ResponseDrugFormDto>> GetActiveDrugFormsAsync();

        Task<(List<DrugFormDto.ResponseDrugFormDto> Items, int TotalCount)> GetFilteredDrugFormsAsync(
            DrugFormDto.DrugFormFilterDto filter);

        Task<(bool Success, string Message, DrugFormDto.ResponseDrugFormDto? Data)> CreateDrugFormAsync(
            DrugFormDto.CreateDrugFormDto dto);

        Task<(bool Success, string Message, List<DrugFormDto.ResponseDrugFormDto> Data)> CreateBulkDrugFormsAsync(
            List<DrugFormDto.CreateDrugFormDto> dtos);

        Task<(bool Success, string Message, DrugFormDto.ResponseDrugFormDto? Data)> UpdateDrugFormAsync(
            DrugFormDto.UpdateDrugFormDto dto);

        Task<(bool Success, string Message)> SoftDeleteDrugFormAsync(Guid formId);

        Task<(bool Success, string Message)> ActivateDrugFormAsync(Guid formId);

        Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds);

        Task<List<string>> GetDrugFormTypesAsync();

        Task<bool> ValidateDrugFormCodeAsync(string formCode, Guid? excludeId = null);

        Task<bool> ValidateDrugFormNameAsync(string formName, Guid? excludeId = null);
    }
}