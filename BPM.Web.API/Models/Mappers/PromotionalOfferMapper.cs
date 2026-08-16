using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class PromotionalOfferMapper
    {
        public static PromotionalOffer ToEntity(this PromotionalOfferCreateDto dto)
        {
            return new PromotionalOffer
            {
                Id = Guid.NewGuid(),
                OfferName = dto.OfferName,
                SupplierId = dto.SupplierId,
                DrugId = dto.DrugId,
                PackagingId = dto.PackagingId,
                DiscountPercentage = dto.DiscountPercentage,
                StartDate = dto.StartDate,
                ExpiryDate = dto.ExpiryDate,
                AllowCombination = dto.AllowCombination,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static PromotionalOfferResponseDto ToDto(this PromotionalOffer entity)
        {
            return new PromotionalOfferResponseDto
            {
                Id = entity.Id,
                OfferName = entity.OfferName,
                SupplierId = entity.SupplierId,
                DrugId = entity.DrugId,
                PackagingId = entity.PackagingId,
                DiscountPercentage = entity.DiscountPercentage,
                StartDate = entity.StartDate,
                ExpiryDate = entity.ExpiryDate,
                AllowCombination = entity.AllowCombination,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static IEnumerable<PromotionalOfferResponseDto> ToDtoList(
            this IEnumerable<PromotionalOffer> entities)
        {
            return entities.Select(ToDto);
        }
    }
}
