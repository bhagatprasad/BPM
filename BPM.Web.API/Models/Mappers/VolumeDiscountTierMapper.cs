using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class VolumeDiscountTierMapper
    {
        public static VolumeDiscountTier ToEntity(this VolumeDiscountTierCreateDto dto)
        {
            return new VolumeDiscountTier
            {
                Id = Guid.NewGuid(),
                SupplierId = dto.SupplierId,
                MinQuantity = dto.MinQuantity,
                MaxQuantity = dto.MaxQuantity,
                DiscountPercentage = dto.DiscountPercentage,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static VolumeDiscountTierResponseDto ToDto(this VolumeDiscountTier entity)
        {
            return new VolumeDiscountTierResponseDto
            {
                Id = entity.Id,
                SupplierId = entity.SupplierId,
                MinQuantity = entity.MinQuantity,
                MaxQuantity = entity.MaxQuantity,
                DiscountPercentage = entity.DiscountPercentage,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static IEnumerable<VolumeDiscountTierResponseDto> ToDtoList(
            this IEnumerable<VolumeDiscountTier> entities)
        {
            return entities.Select(ToDto);
        }
    }
}
