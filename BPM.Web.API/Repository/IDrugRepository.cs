using System;

using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDrugRepository
    {
        Task<List<Drug>> GetAllDrugsAsync();

        Task<Drug?> GetDrugByIdAsync(Guid drugId);

        Task<bool> InsertDrugAsync(Drug drugs);

        Task<bool> UpdateDrugAsync(Drug drugs);

        Task<bool> DeleteDrugAsync(Guid drugId);
    }
}
