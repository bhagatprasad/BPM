using BPM.Web.API.Models.DTOs.Discount;

namespace BPM.Web.API.Services
{
    public interface ISupplierDiscountService
    {
        Task<SupplierDiscountResponseDto> CreateAsync(SupplierDiscountCreateDto dto);

        Task<IEnumerable<SupplierDiscountResponseDto>> GetAllAsync();

        Task<SupplierDiscountResponseDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<SupplierDiscountResponseDto>> GetBySupplierAsync(Guid supplierId);
    }
}