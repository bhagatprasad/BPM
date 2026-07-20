using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDrugUomRepository
    {
        Task<List<DrugUom>> GetAllAsync();

        Task<DrugUom?> GetByIdAsync(Guid uomId);

        Task<List<DrugUom>> GetByDrugIdAsync(Guid drugId);

        Task<bool> InsertAsync(DrugUom drugUom);

        Task<bool> UpdateAsync(DrugUom drugUom);

        Task<bool> DeleteAsync(Guid uomId);
    }
}