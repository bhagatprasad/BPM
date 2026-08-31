using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Repositories
{
    public interface IDrugUomRepository
    {
        Task<List<DrugUom>> GetAllDrugUomsAsync();
        Task<DrugUom?> GetDrugUomByIdAsync(Guid uomId);
        Task<List<DrugUom>> GetDrugUomsByDrugIdAsync(Guid drugId);
        Task<DrugUom?> GetDrugUomByCodeAsync(Guid drugId,string uomCode);      
        Task<List<DrugUom>> GetBaseUnitsByDrugIdAsync(Guid drugId);
        Task<List<DrugUom>> GetPurchaseUomsByDrugIdAsync(Guid drugId);
        Task<List<DrugUom>> GetSalesUomsByDrugIdAsync(Guid drugId);
        Task<bool> InsertDrugUomAsync(DrugUom drugUom); 
        Task<bool> UpdateDrugUomAsync(DrugUom drugUom);  
        Task<bool> DeleteDrugUomAsync(Guid uomId);

        // VALIDATION
        Task<bool> DrugUomExistsAsync(
            Guid drugId,
            string uomCode,
            Guid? excludeUomId = null);

        // CHECK CHILD UOMS
        Task<bool> HasChildUomsAsync(Guid parentUomId);
    }
}
