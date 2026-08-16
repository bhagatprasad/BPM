using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface ISupplierDiscountRepository
    {
        Task<SupplierDiscount> CreateAsync(SupplierDiscount supplierDiscount);

        Task<IEnumerable<SupplierDiscount>> GetAllAsync();

        Task<SupplierDiscount?> GetByIdAsync(Guid id);

        Task<IEnumerable<SupplierDiscount>> GetBySupplierAsync(Guid supplierId);
    }
}
