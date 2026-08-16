using BPM.Web.API.Models.DTOs.Discount;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class SupplierDiscountMapper
    {
        public static SupplierDiscount ToEntity(this SupplierDiscountCreateDto dto)
        {
            return new SupplierDiscount
            {
                Id = Guid.NewGuid(),
                SupplierId = dto.SupplierId,
                DiscountPercentage = dto.DiscountPercentage,
                ValidFrom = dto.ValidFrom,
                ValidTo = dto.ValidTo,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static SupplierDiscountResponseDto ToDto(this SupplierDiscount entity)
        {
            return new SupplierDiscountResponseDto
            {
                Id = entity.Id,
                SupplierId = entity.SupplierId,
                DiscountPercentage = entity.DiscountPercentage,
                ValidFrom = entity.ValidFrom,
                ValidTo = entity.ValidTo,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static IEnumerable<SupplierDiscountResponseDto> ToDtoList(
            this IEnumerable<SupplierDiscount> entities)
        {
            return entities.Select(ToDto);
        }
    }
}
