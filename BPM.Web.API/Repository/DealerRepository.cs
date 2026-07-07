using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class DealerRepository : IDealerRepository
    {
        private readonly ApplicationDbContext _context;

        public DealerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Dealer>> GetAllDealersAsync()
        {
            return await _context.Dealers
                .OrderBy(x => x.DealershipName)
                .ToListAsync();
        }

        public async Task<Dealer?> GetDealerByIdAsync(Guid dealerId)
        {
            return await _context.Dealers
                .FirstOrDefaultAsync(x => x.Id == dealerId);
        }

        public async Task<bool> InsertDealerAsync(Dealer dealer)
        {
            dealer.CreatedOn = DateTime.UtcNow;
            dealer.IsActive = true;

            _context.Dealers.Add(dealer);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDealerAsync(Dealer dealer)
        {
            var existingDealer = await _context.Dealers.FindAsync(dealer.Id);

            if (existingDealer == null)
                return false;

            existingDealer.DealershipName = dealer.DealershipName;
            existingDealer.RegistrationNumber = dealer.RegistrationNumber;
            existingDealer.TradeLicenseNumber = dealer.TradeLicenseNumber;
            existingDealer.GSTNumber = dealer.GSTNumber;
            existingDealer.ContactPerson = dealer.ContactPerson;
            existingDealer.Email = dealer.Email;
            existingDealer.Phone = dealer.Phone;
            existingDealer.AlternatePhone = dealer.AlternatePhone;
            existingDealer.AddressLine1 = dealer.AddressLine1;
            existingDealer.AddressLine2 = dealer.AddressLine2;
            existingDealer.City = dealer.City;
            existingDealer.State = dealer.State;
            existingDealer.Country = dealer.Country;
            existingDealer.PostalCode = dealer.PostalCode;
            existingDealer.Website = dealer.Website;
            existingDealer.IsActive = dealer.IsActive;
            existingDealer.ModifiedBy = dealer.ModifiedBy;
            existingDealer.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDealerAsync(Guid dealerId)
        {
            var dealer = await _context.Dealers.FindAsync(dealerId);

            if (dealer == null)
                return false;

            _context.Dealers.Remove(dealer);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}