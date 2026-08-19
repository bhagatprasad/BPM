using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class DistributorRepository : IDistributorRepository
    {
        private readonly ApplicationDbContext _context;
        public DistributorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Distributor>> GetAllDistributorsAsync()
        {
            return await _context.Distributors.OrderBy(x => x.DistributorId).ToListAsync();
        }

        public async Task<Distributor?> GetDistributorByIdAsync(Guid distributorId)
        {
            return await _context.Distributors.FirstOrDefaultAsync(x => x.DistributorId == distributorId);
        }

        public async Task<bool> InsertDistributorAsync(Distributor distributor)
        {
            if (distributor == null)
                throw new ArgumentNullException(nameof(distributor));

            try
            {
                distributor.CreatedOn = DateTime.UtcNow;
                distributor.ModifiedOn = DateTime.UtcNow;
                _context.Distributors.Add(distributor);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {                
                throw; 
            }
        }
        public async Task<bool> UpdateDistributorAsync(Distributor distributor)
        {
            var existingDistributor = await _context.Distributors.FindAsync(distributor.DistributorId);
            if (existingDistributor == null)
            {
                return false;
            }
            existingDistributor.DistributorName = distributor.DistributorName;
            existingDistributor.ContactPerson = distributor.ContactPerson;
            existingDistributor.Email = distributor.Email;
            existingDistributor.Phone = distributor.Phone;
            existingDistributor.AlternatePhone = distributor.AlternatePhone;
            existingDistributor.AddressLine1 = distributor.AddressLine1;
            existingDistributor.AddressLine2 = distributor.AddressLine2;
            existingDistributor.City = distributor.City;
            existingDistributor.State = distributor.State;
            existingDistributor.Country = distributor.Country;
            existingDistributor.PostalCode = distributor.PostalCode;
            existingDistributor.Website = distributor.Website;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDistributorAsync(Guid distributorId)
        {
            var distributor = await _context.Distributors.FindAsync(distributorId);

            if (distributor == null)
            {
                return false;
            }

            distributor.IsActive = false;
            distributor.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
