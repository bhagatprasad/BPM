using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDrugPackagingRepository
    {
        Task<List<DrugPackaging>> GetAllAsync();
        Task<DrugPackaging?> GetByIdAsync(Guid packagingId);
        Task<List<DrugPackaging>> GetByDrugIdAsync(Guid drugId);
        Task<List<DrugPackaging>> GetByPackageUomIdAsync(Guid packageUomId);
        Task<List<DrugPackaging>> GetByContainsUomIdAsync(Guid containsUomId);
        Task<DrugPackaging?> GetByBarcodeAsync(string barcode);
        Task<List<DrugPackaging>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<(List<DrugPackaging> Items, int TotalCount)> GetFilteredAsync(DrugPackagingFilterDto filter);
        Task<bool> InsertAsync(DrugPackaging packaging);
        Task<bool> UpdateAsync(DrugPackaging packaging);
        Task<bool> DeleteAsync(Guid packagingId);
        Task<bool> ExistsByBarcodeAsync(string barcode, Guid? excludeId = null);
        Task<bool> HasActivePackagingAsync(Guid drugId);
        Task<decimal> GetTotalPackagesByDrugAsync(Guid drugId);
        Task<bool> ValidateUomCompatibilityAsync(Guid packageUomId, Guid containsUomId);
    }
}
