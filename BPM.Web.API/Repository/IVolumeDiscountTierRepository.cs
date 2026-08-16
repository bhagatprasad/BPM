using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IVolumeDiscountTierRepository
    {
        Task<VolumeDiscountTier> CreateAsync(VolumeDiscountTier volumeDiscountTier);

        Task<IEnumerable<VolumeDiscountTier>> GetAllAsync();

        Task<VolumeDiscountTier?> GetByIdAsync(Guid id);

        Task<IEnumerable<VolumeDiscountTier>> GetBySupplierAsync(Guid supplierId);
    }
}
