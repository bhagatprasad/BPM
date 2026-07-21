using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Service
{
    public interface IDrugUomService
    {
        Task<List<DrugUom>> GetAllAsync();
        Task<DrugUom?> GetByIdAsync(Guid uomId);
        Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId);
        Task<DrugUom?> GetByCodeAsync(Guid drugId, string uomCode);
        Task<List<DrugUom>> GetBaseUnitsByDrugIdAsync(Guid drugId);
        Task<List<DrugUom>> GetPurchaseUomsByDrugIdAsync(Guid drugId);
        Task<List<DrugUom>> GetSalesUomsByDrugIdAsync(Guid drugId);
        Task<(bool Success, string Message)> InsertAsync(DrugUom drugUom);
        Task<(bool Success, string Message)> UpdateAsync(DrugUom drugUom);
        Task<(bool Success, string Message)> DeleteAsync(Guid uomId);
        Task<bool> ValidateUomHierarchyAsync(DrugUom drugUom);
    }
}