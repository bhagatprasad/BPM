using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _repository.GetAllSuppliersAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(Guid supplierId)
        {
            return await _repository.GetSupplierByIdAsync(supplierId);
        }

        public async Task<bool> InsertSupplierAsync(Supplier supplier)
        {
            return await _repository.InsertSupplierAsync(supplier);
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            return await _repository.UpdateSupplierAsync(supplier);
        }

        public async Task<bool> DeleteSupplierAsync(Guid supplierId)
        {
            return await _repository.DeleteSupplierAsync(supplierId);
        }
    }
}
