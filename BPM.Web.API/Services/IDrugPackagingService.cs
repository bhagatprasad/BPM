using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IDrugPackagingService
    {
        Task<List<DrugPackagingDto>> GetAllAsync();
        Task<DrugPackagingDto?> GetByIdAsync(Guid packagingId);
        Task<List<DrugPackagingDto>> GetByDrugIdAsync(Guid drugId);
        Task<DrugPackagingDto?> GetByBarcodeAsync(string barcode);
        Task<(List<DrugPackagingDto> Items, int TotalCount)> GetFilteredAsync(DrugPackagingFilterDto filter);
        Task<(bool Success, string Message, DrugPackagingDto? Data)> CreateAsync(CreateDrugPackagingDto dto);
        Task<(bool Success, string Message, DrugPackagingDto? Data)> UpdateAsync(UpdateDrugPackagingDto dto);
        Task<(bool Success, string Message)> DeleteAsync(Guid packagingId);
        Task<(bool Success, string Message)> ValidatePackagingAsync(CreateDrugPackagingDto dto);
        Task<decimal> CalculatePackagePriceAsync(Guid packageUomId, Guid containsUomId, int quantity, decimal unitPrice);
        Task<Dictionary<Guid, decimal>> GetPackagingSummaryByDrugAsync(Guid drugId);
    }
}
