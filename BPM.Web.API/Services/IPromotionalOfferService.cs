using BPM.Web.API.Models.DTOs.Discount;

namespace BPM.Web.API.Services
{
    public interface IPromotionalOfferService
    {
        Task<PromotionalOfferResponseDto> CreateAsync(PromotionalOfferCreateDto dto);

        Task<IEnumerable<PromotionalOfferResponseDto>> GetAllAsync();

        Task<PromotionalOfferResponseDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<PromotionalOfferResponseDto>> GetBySupplierAsync(Guid supplierId);

        Task<IEnumerable<PromotionalOfferResponseDto>> GetByDrugAsync(Guid drugId);

        Task<IEnumerable<PromotionalOfferResponseDto>> GetActiveOffersAsync();
    }
}
