using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DiscountCodeMapper
    {
        public static DiscountCode ToEntity(this DiscountCodeCreateDto dto)
        {
            return new DiscountCode
            {
                Id = Guid.NewGuid(),
                DiscountCodeValue = dto.DiscountCode,
                DiscountPercentage = dto.DiscountPercentage,
                SupplierId = dto.SupplierId,
                StartDate = dto.StartDate,
                ExpiryDate = dto.ExpiryDate,
                RequiresApproval = dto.RequiresApproval,
                IsApproved = dto.IsApproved,
                AllowCombination = dto.AllowCombination,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static DiscountCodeResponseDto ToDto(this DiscountCode entity)
        {
            return new DiscountCodeResponseDto
            {
                Id = entity.Id,
                DiscountCode = entity.DiscountCodeValue,
                DiscountPercentage = entity.DiscountPercentage,
                SupplierId = entity.SupplierId,
                StartDate = entity.StartDate,
                ExpiryDate = entity.ExpiryDate,
                RequiresApproval = entity.RequiresApproval,
                IsApproved = entity.IsApproved,
                AllowCombination = entity.AllowCombination,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static IEnumerable<DiscountCodeResponseDto> ToDtoList(
            this IEnumerable<DiscountCode> entities)
        {
            return entities.Select(ToDto);
        }
    }
}
