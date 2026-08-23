using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Repositories
{
    public interface IDrugRepository
    {
        Task<List<DrugEntity>> GetAllDrugsAsync();

        Task<DrugEntity?> GetDrugByIdAsync(Guid drugId);

        Task<bool> InsertDrugAsync(DrugEntity drug);

        Task<bool> UpdateDrugAsync(DrugEntity drug);

        Task<bool> DeleteDrugAsync(Guid drugId);
    }
}
