using BPM.Web.Drug.API.Models.DTOs;

namespace BPM.Web.Drug.API.Services
{
    public interface IDrugService
    {
        Task<List<DrugDto.ResponseDrugDto>> GetAllDrugsAsync();

        Task<DrugDto.ResponseDrugDto?> GetDrugByIdAsync(Guid drugId);

        Task<bool> CreateDrugAsync(DrugDto.CreateDrugDto dto);

        Task<bool> UpdateDrugAsync(DrugDto.UpdateDrugDto dto);

        Task<bool> DeleteDrugAsync(Guid drugId);
    }
}
