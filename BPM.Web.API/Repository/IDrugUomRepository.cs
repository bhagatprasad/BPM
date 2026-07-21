using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDrugUomRepository
    {
        Task<List<DrugUom>> GetAllAsync();
        Task<DrugUom?> GetByIdAsync(Guid uomId);
        Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId);
        Task<DrugUom?> GetByCodeAsync(Guid drugId, string uomCode);
        Task<List<DrugUom>> GetBaseUnitsByDrugIdAsync(Guid drugId);
        Task<List<DrugUom>> GetPurchaseUomsByDrugIdAsync(Guid drugId);
        Task<List<DrugUom>> GetSalesUomsByDrugIdAsync(Guid drugId);
        Task<bool> InsertAsync(DrugUom drugUom);
        Task<bool> UpdateAsync(DrugUom drugUom);
        Task<bool> DeleteAsync(Guid uomId);
        Task<bool> ExistsAsync(Guid drugId, string uomCode, Guid? excludeUomId = null);
        Task<bool> HasChildUomsAsync(Guid parentUomId);
    }
}