using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface ISupplierService
    {
        Task<List<SupplierDto>> GetAllSuppliersAsync();

        Task<SupplierDto?> GetSupplierByIdAsync(Guid supplierId);

        Task<bool> InsertSupplierAsync(CreateSupplierDto supplierDto);

        Task<bool> UpdateSupplierAsync(UpdateSupplierDto supplierDto);

        Task<bool> DeleteSupplierAsync(Guid supplierId);
    }
}
