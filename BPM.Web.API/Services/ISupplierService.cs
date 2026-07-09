using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Services
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllSuppliersAsync();

        Task<Supplier?> GetSupplierByIdAsync(Guid supplierId);

        Task<bool> InsertSupplierAsync(Supplier supplier);

        Task<bool> UpdateSupplierAsync(Supplier supplier);

        Task<bool> DeleteSupplierAsync(Guid supplierId);
    }
}
