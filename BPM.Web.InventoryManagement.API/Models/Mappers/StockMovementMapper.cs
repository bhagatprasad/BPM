using BPM.Web.InventoryManagement.API.Models.DTOs;
using BPM.Web.InventoryManagement.API.Models.Entities;

namespace BPM.Web.InventoryManagement.API.Models.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement ToEntity(this StockMovementCreateDto dto)
        {
            return new StockMovement
            {
                Id = Guid.NewGuid(),
                InventoryId = dto.InventoryId,
                DrugId = dto.DrugId,
                PackagingId = dto.PackagingId,
                BatchId = dto.BatchId,
                WarehouseId = dto.WarehouseId,
                DistributorId = dto.DistributorId,
                MovementType = dto.MovementType,
                Quantity = dto.Quantity,
                QuantityBefore = dto.QuantityBefore,
                QuantityAfter = dto.QuantityAfter,
                ReferenceType = dto.ReferenceType,
                ReferenceId = dto.ReferenceId,
                UnitCost = dto.UnitCost,
                Remarks = dto.Remarks,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static StockMovement ToEntity(this StockMovementUpdateDto dto, StockMovement existingEntity)
        {
            if (existingEntity == null)
                throw new ArgumentNullException(nameof(existingEntity));

            existingEntity.InventoryId = dto.InventoryId;
            existingEntity.DrugId = dto.DrugId;
            existingEntity.PackagingId = dto.PackagingId;
            existingEntity.BatchId = dto.BatchId;
            existingEntity.WarehouseId = dto.WarehouseId;
            existingEntity.DistributorId = dto.DistributorId;
            existingEntity.MovementType = dto.MovementType;
            existingEntity.Quantity = dto.Quantity;
            existingEntity.QuantityBefore = dto.QuantityBefore;
            existingEntity.QuantityAfter = dto.QuantityAfter;
            existingEntity.ReferenceType = dto.ReferenceType;
            existingEntity.ReferenceId = dto.ReferenceId;
            existingEntity.UnitCost = dto.UnitCost;
            existingEntity.Remarks = dto.Remarks;

            return existingEntity;
        }

        public static StockMovementResponseDto ToDto(this StockMovement entity)
        {
            return new StockMovementResponseDto
            {
                Id = entity.Id,
                InventoryId = entity.InventoryId,
                DrugId = entity.DrugId,
                PackagingId = entity.PackagingId,
                BatchId = entity.BatchId,
                WarehouseId = entity.WarehouseId,
                DistributorId = entity.DistributorId,
                MovementType = entity.MovementType,
                Quantity = entity.Quantity,
                QuantityBefore = entity.QuantityBefore,
                QuantityAfter = entity.QuantityAfter,
                ReferenceType = entity.ReferenceType,
                ReferenceId = entity.ReferenceId,
                UnitCost = entity.UnitCost,
                Remarks = entity.Remarks,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
            };
        }

        public static List<StockMovementResponseDto> ToDtoList(this IEnumerable<StockMovement> entities)
        {
            return entities.Select(ToDto).ToList();
        }

    }
}