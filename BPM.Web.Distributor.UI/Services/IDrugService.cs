using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IDrugService
    {
        Task<List<DrugDto>> GetAllDrugsAsync();

        Task<DrugDto?> GetDrugByIdAsync(Guid drugId);

        Task<bool> InsertDrugAsync(CreateDrugDto drugDto);

        Task<bool> UpdateDrugAsync(UpdateDrugDto drugDto);

        Task<bool> DeleteDrugAsync(Guid drugId);
    }
}
