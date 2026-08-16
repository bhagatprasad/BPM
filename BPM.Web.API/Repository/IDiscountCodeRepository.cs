using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDiscountCodeRepository
    {
        Task<DiscountCode> CreateAsync(DiscountCode discountCode);

        Task<IEnumerable<DiscountCode>> GetAllAsync();

        Task<DiscountCode?> GetByIdAsync(Guid id);

        Task<DiscountCode?> GetByCodeAsync(string discountCode);

        Task<IEnumerable<DiscountCode>> GetBySupplierAsync(Guid supplierId);

        Task<IEnumerable<DiscountCode>> GetActiveCodesAsync();
    }
}
