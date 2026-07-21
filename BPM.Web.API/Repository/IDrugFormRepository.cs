using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDrugFormRepository
    {
        Task<List<DrugForm>> GetAllAsync();
        Task<DrugForm?> GetByIdAsync(Guid formId);
        Task<DrugForm?> GetByCodeAsync(string formCode);
        Task<DrugForm?> GetByNameAsync(string formName);
        Task<List<DrugForm>> GetByTypeAsync(string formType);
        Task<List<DrugForm>> GetActiveFormsAsync();
        Task<(List<DrugForm> Items, int TotalCount)> GetFilteredAsync(DrugFormFilterDto filter);
        Task<bool> InsertAsync(DrugForm drugForm);
        Task<bool> InsertBulkAsync(List<DrugForm> drugForms);
        Task<bool> UpdateAsync(DrugForm drugForm);
        Task<bool> DeleteAsync(Guid formId);
        Task<bool> SoftDeleteAsync(Guid formId);
        Task<bool> ExistsByCodeAsync(string formCode, Guid? excludeId = null);
        Task<bool> ExistsByNameAsync(string formName, Guid? excludeId = null);
        Task<bool> HasDrugsAsync(Guid formId);
        Task<int> GetDrugCountByFormAsync(Guid formId);
        Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds);
    }
}
