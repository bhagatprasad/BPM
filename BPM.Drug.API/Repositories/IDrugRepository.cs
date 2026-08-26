namespace BPM.Web.Drug.API.Repositories
{
    public interface IDrugRepository
    {
        Task<List<BPM.Web.Drug.API.Models.Entities.Drug>> GetAllDrugsAsync();

        Task<BPM.Web.Drug.API.Models.Entities.Drug?> GetDrugByIdAsync(Guid drugId);

        Task<bool> InsertDrugAsync(BPM.Web.Drug.API.Models.Entities.Drug drug);

        Task<bool> UpdateDrugAsync(BPM.Web.Drug.API.Models.Entities.Drug drug);

        Task<bool> DeleteDrugAsync(Guid drugId);
    }
}
