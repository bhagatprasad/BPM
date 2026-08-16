using BPM.Web.API.Models.DTOs.Discount;

namespace BPM.Web.API.Services
{
    public interface IVolumeDiscountTierService
    {
        Task<VolumeDiscountTierResponseDto> CreateAsync(VolumeDiscountTierCreateDto dto);

        Task<IEnumerable<VolumeDiscountTierResponseDto>> GetAllAsync();

        Task<VolumeDiscountTierResponseDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<VolumeDiscountTierResponseDto>> GetBySupplierAsync(Guid supplierId);
    }
}
