using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repositories
{
    public class PromotionalOfferRepository : IPromotionalOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public PromotionalOfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PromotionalOffer> CreateAsync(PromotionalOffer promotionalOffer)
        {
            await _context.PromotionalOffers.AddAsync(promotionalOffer);
            await _context.SaveChangesAsync();

            return promotionalOffer;
        }

        public async Task<IEnumerable<PromotionalOffer>> GetAllAsync()
        {
            return await _context.PromotionalOffers
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<PromotionalOffer?> GetByIdAsync(Guid id)
        {
            return await _context.PromotionalOffers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<IEnumerable<PromotionalOffer>> GetBySupplierAsync(Guid supplierId)
        {
            return await _context.PromotionalOffers
                .AsNoTracking()
                .Where(x => x.SupplierId == supplierId && x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<PromotionalOffer>> GetByDrugAsync(Guid drugId)
        {
            return await _context.PromotionalOffers
                .AsNoTracking()
                .Where(x => x.DrugId == drugId && x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<PromotionalOffer>> GetActiveOffersAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.PromotionalOffers
                .AsNoTracking()
                .Where(x => x.IsActive &&
                            x.StartDate <= now &&
                            x.ExpiryDate >= now)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }
    }
}