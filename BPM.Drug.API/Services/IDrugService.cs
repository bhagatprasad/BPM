using BPM.Web.Drug.API.Models.DTOs;

namespace BPM.Web.Drug.API.Services
{
    public interface IDrugService
    {
        Task<List<ResponseDrugDto>> GetAllDrugsAsync();

        Task<ResponseDrugDto?> GetDrugByIdAsync(Guid drugId);

        Task<bool> CreateDrugAsync(CreateDrugDto dto);

        Task<bool> UpdateDrugAsync(UpdateDrugDto dto);

        Task<bool> DeleteDrugAsync(Guid drugId);
    }
}
