using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Models.Mappers
{
    public static class InventoryMapper
    {
        public static Inventory ToEntity(InventoryCreateDto dto)
        {
            return new Inventory
            {
                Id = Guid.NewGuid(),
                DrugId = dto.DrugId,
                PackagingId = dto.PackagingId,
                BatchId = dto.BatchId,
                WarehouseId = dto.WarehouseId,
                DistributorId = dto.DistributorId,
                Quantity = dto.Quantity,
                ReservedQuantity = dto.ReservedQuantity,
                AvailableQuantity = dto.Quantity - dto.ReservedQuantity,
                ReorderLevel = dto.ReorderLevel,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Inventory ToEntity(InventoryUpdateDto dto,Inventory entity)
        {
            entity.DrugId = dto.DrugId;
            entity.PackagingId = dto.PackagingId;
            entity.BatchId = dto.BatchId;
            entity.WarehouseId = dto.WarehouseId;
            entity.DistributorId = dto.DistributorId;
            entity.Quantity = dto.Quantity;
            entity.ReservedQuantity = dto.ReservedQuantity;
            entity.AvailableQuantity = dto.Quantity - dto.ReservedQuantity;
            entity.ReorderLevel = dto.ReorderLevel;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ModifiedOn = DateTime.UtcNow;

            return entity;
        }

        public static InventoryResponseDto ToDto(Inventory entity)
        {
            return new InventoryResponseDto
            {
                Id = entity.Id,
                DrugId = entity.DrugId,
                PackagingId = entity.PackagingId,
                BatchId = entity.BatchId,
                WarehouseId = entity.WarehouseId,
                DistributorId = entity.DistributorId,
                Quantity = entity.Quantity,
                ReservedQuantity = entity.ReservedQuantity,
                AvailableQuantity = entity.AvailableQuantity,
                ReorderLevel = entity.ReorderLevel,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static List<InventoryResponseDto> ToDtoList(IEnumerable<Inventory> entities)
        {
            return entities.Select(ToDto).ToList();
        }
    }
}
