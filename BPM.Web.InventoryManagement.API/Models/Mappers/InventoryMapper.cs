using BPM.Web.InventoryManagement.API.Models.DTOs;
using BPM.Web.InventoryManagement.API.Models.Entities;

namespace BPM.Web.InventoryManagement.API.Models.Mappers
{
    public static class InventoryMapper
    {
        public static Inventory ToEntity(this InventoryCreateDto dto)
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

        public static Inventory ToEntity(this InventoryUpdateDto dto, Inventory entity)
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

        public static InventoryResponseDto ToDto(this Inventory entity)
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

        public static List<InventoryResponseDto> ToDtoList(this IEnumerable<Inventory> entities)
        {
            return entities.Select(ToDto).ToList();
        }


        public static List<Inventory> ToInventoryEntries(this DistributorDto distributor, WarehouseResponseDto warehouse, List<DrugDto> drugs)
        {
            List<Inventory> inventorys = new List<Inventory>();

            if (drugs.Any())
            {
                foreach (DrugDto drug in drugs)
                {
                    if (drug.DrugPackagings.Any())
                    {
                        foreach (var item in drug.DrugPackagings)
                        {
                            inventorys.Add(new Inventory()
                            {
                                AvailableQuantity = 10 * 10,
                                BatchId = Guid.NewGuid(),
                                CreatedBy = distributor.CreatedBy,
                                CreatedOn = distributor.CreatedOn,
                                DistributorId = distributor.DistributorId,
                                DrugId = drug.DrugId,
                                IsActive = drug.IsActive,
                                ModifiedBy = distributor.ModifiedBy,
                                ModifiedOn = distributor.ModifiedOn,
                                PackagingId = item.PackagingId,
                                Quantity = 10 * 10,
                                ReorderLevel = 1,
                                ReservedQuantity = 10 * 10,
                                WarehouseId = warehouse.Id,
                                Id = Guid.NewGuid(),
                            });
                        }
                    }

                }
            }

            return inventorys;

        }
    }
}
