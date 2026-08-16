using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IPromotionalOfferRepository
    {
        Task<PromotionalOffer> CreateAsync(PromotionalOffer promotionalOffer);

        Task<IEnumerable<PromotionalOffer>> GetAllAsync();

        Task<PromotionalOffer?> GetByIdAsync(Guid id);

        Task<IEnumerable<PromotionalOffer>> GetBySupplierAsync(Guid supplierId);

        Task<IEnumerable<PromotionalOffer>> GetByDrugAsync(Guid drugId);

        Task<IEnumerable<PromotionalOffer>> GetActiveOffersAsync();
    }
}
