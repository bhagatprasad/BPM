using BPM.Web.Drug.API.Models.DTOs;

namespace BPM.Web.Drug.API.Services
{
    public interface IDrugUomService
    {
        Task<List<DrugUomDto.ResponseDrugUomDto>> GetAllDrugUomsAsync();
        Task<DrugUomDto.ResponseDrugUomDto?> GetDrugUomByIdAsync(Guid uomId);
        Task<List<DrugUomDto.ResponseDrugUomDto>> GetDrugUomsByDrugIdAsync(Guid drugId);
        Task<DrugUomDto.ResponseDrugUomDto?> GetDrugUomByCodeAsync(Guid drugId, string uomCode);
        Task<List<DrugUomDto.ResponseDrugUomDto>> GetBaseUnitsByDrugIdAsync(Guid drugId);
        Task<List<DrugUomDto.ResponseDrugUomDto>> GetPurchaseUomsByDrugIdAsync(Guid drugId);
        Task<List<DrugUomDto.ResponseDrugUomDto>> GetSalesUomsByDrugIdAsync(Guid drugId);
        Task<bool> CreateDrugUomAsync(DrugUomDto.CreateDrugUomDto dto);
        Task<bool> UpdateDrugUomAsync(DrugUomDto.UpdateDrugUomDto dto);
        Task<bool> DeleteDrugUomAsync(Guid uomId);
    }
}