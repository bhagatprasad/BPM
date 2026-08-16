using BPM.Web.API.Models.DTOs.Discount;

namespace BPM.Web.API.Services
{
    public interface IDiscountCodeService
    {
        Task<DiscountCodeResponseDto> CreateAsync(DiscountCodeCreateDto dto);

        Task<IEnumerable<DiscountCodeResponseDto>> GetAllAsync();

        Task<DiscountCodeResponseDto?> GetByIdAsync(Guid id);

        Task<DiscountCodeResponseDto?> GetByCodeAsync(string discountCode);

        Task<IEnumerable<DiscountCodeResponseDto>> GetBySupplierAsync(Guid supplierId);

        Task<IEnumerable<DiscountCodeResponseDto>> GetActiveCodesAsync();
    }
}
