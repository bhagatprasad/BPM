using BPM.Web.Drug.API.Models.DTOs;

namespace BPM.Web.Drug.API.Services
{
    public interface IDrugPackagingService
    {
        Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetAllAsync();
        Task<DrugPackagingDto.ResponseDrugPackagingDto?> GetByIdAsync(Guid packagingId);
        Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByDrugIdAsync(Guid drugId);
        Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByPackageUomIdAsync(Guid packageUomId);
        Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByContainsUomIdAsync(Guid containsUomId);
        Task<DrugPackagingDto.ResponseDrugPackagingDto?> GetByBarcodeAsync(string barcode);
        Task<List<DrugPackagingDto.ResponseDrugPackagingDto>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<(List<DrugPackagingDto.ResponseDrugPackagingDto> Items, int TotalCount)> GetFilteredAsync(DrugPackagingDto.DrugPackagingFilterDto filter);
        Task<bool> CreateAsync(DrugPackagingDto.CreateDrugPackagingDto dto);
        Task<bool> UpdateAsync(DrugPackagingDto.UpdateDrugPackagingDto dto);
        Task<bool> DeleteAsync(Guid packagingId);
        Task<decimal> GetTotalPackagesByDrugAsync(Guid drugId);
    }
}
