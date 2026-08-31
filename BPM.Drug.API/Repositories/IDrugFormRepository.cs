using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Repositories.Interfaces
{
    public interface IDrugFormRepository
    {
        Task<List<DrugFormEntity>> GetAllDrugFormsAsync();
        Task<DrugFormEntity?> GetDrugFormByIdAsync(Guid formId);
        Task<DrugFormEntity?> GetDrugFormByCodeAsync(string formCode);
        Task<DrugFormEntity?> GetDrugFormByNameAsync(string formName);
        Task<List<DrugFormEntity>> GetDrugFormsByTypeAsync(string formType);
        Task<List<DrugFormEntity>> GetActiveDrugFormsAsync();
        Task<(List<DrugFormEntity> Items, int TotalCount)> GetFilteredDrugFormsAsync(DrugFormDto.DrugFormFilterDto filter);
        Task<bool> InsertDrugFormAsync(DrugFormEntity drugForm);
        Task<bool> InsertBulkDrugFormsAsync(List<DrugFormEntity> drugForms);
        Task<bool> UpdateDrugFormAsync(DrugFormEntity drugForm);
        Task<bool> SoftDeleteDrugFormAsync(Guid formId);
        Task<bool> DrugFormCodeExistsAsync(string formCode, Guid? excludeId = null);
        Task<bool> DrugFormNameExistsAsync(string formName, Guid? excludeId = null);
        Task<bool> HasDrugsAsync(Guid formId);
        Task<int> GetDrugCountByFormAsync(Guid formId);
        Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds);
    }
}