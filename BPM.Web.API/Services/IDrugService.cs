using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Service
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