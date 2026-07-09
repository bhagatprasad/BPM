using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .OrderBy(x => x.SupplierName)
                .ToListAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(Guid supplierId)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(x => x.SupplierId == supplierId);
        }

        public async Task<bool> InsertSupplierAsync(Supplier supplier)
        {
            supplier.SupplierId = Guid.NewGuid();
            supplier.CreatedOn = DateTime.UtcNow;
            supplier.IsActive = true;

            _context.Suppliers.Add(supplier);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers
                .FirstOrDefaultAsync(x => x.SupplierId == supplier.SupplierId);

            if (existingSupplier == null)
                return false;

            existingSupplier.SupplierCode = supplier.SupplierCode;
            existingSupplier.SupplierName = supplier.SupplierName;
            existingSupplier.ContactPerson = supplier.ContactPerson;
            existingSupplier.Email = supplier.Email;
            existingSupplier.Phone = supplier.Phone;
            existingSupplier.AlternatePhone = supplier.AlternatePhone;
            existingSupplier.GSTNumber = supplier.GSTNumber;
            existingSupplier.DrugLicenseNumber = supplier.DrugLicenseNumber;
            existingSupplier.AddressLine1 = supplier.AddressLine1;
            existingSupplier.AddressLine2 = supplier.AddressLine2;
            existingSupplier.City = supplier.City;
            existingSupplier.State = supplier.State;
            existingSupplier.Country = supplier.Country;
            existingSupplier.PostalCode = supplier.PostalCode;
            existingSupplier.Website = supplier.Website;
            existingSupplier.IsActive = supplier.IsActive;
            existingSupplier.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierAsync(Guid supplierId)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(x => x.SupplierId == supplierId);

            if (supplier == null)
                return false;

            _context.Suppliers.Remove(supplier);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
