using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext _context;

        public ManufacturerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            return await _context.Manufacturers
                .OrderBy(x => x.ManufacturerName)
                .ToListAsync();
        }

        public async Task<Manufacturer?> GetManufacturerByIdAsync(Guid manufacturerId)
        {
            return await _context.Manufacturers
                .FirstOrDefaultAsync(x => x.Id == manufacturerId);
        }

        public async Task<bool> InsertManufacturerAsync(Manufacturer manufacturer)
        {
            manufacturer.CreatedOn = DateTime.UtcNow;
            manufacturer.IsActive = true;

            _context.Manufacturers.Add(manufacturer);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            var existingManufacturer = await _context.Manufacturers.FindAsync(manufacturer.Id);

            if (existingManufacturer == null)
                return false;

            existingManufacturer.ManufacturerCode = manufacturer.ManufacturerCode;
            existingManufacturer.ManufacturerName = manufacturer.ManufacturerName;
            existingManufacturer.ContactPerson = manufacturer.ContactPerson;
            existingManufacturer.Email = manufacturer.Email;
            existingManufacturer.Phone = manufacturer.Phone;
            existingManufacturer.AddressLine1 = manufacturer.AddressLine1;
            existingManufacturer.AddressLine2 = manufacturer.AddressLine2;
            existingManufacturer.City = manufacturer.City;
            existingManufacturer.State = manufacturer.State;
            existingManufacturer.Country = manufacturer.Country;
            existingManufacturer.PostalCode = manufacturer.PostalCode;
            existingManufacturer.Website = manufacturer.Website;
            existingManufacturer.IsActive = manufacturer.IsActive;
            existingManufacturer.ModifiedBy = manufacturer.ModifiedBy;
            existingManufacturer.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteManufacturerAsync(Guid manufacturerId)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(manufacturerId);

            if (manufacturer == null)
                return false;

            _context.Manufacturers.Remove(manufacturer);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}