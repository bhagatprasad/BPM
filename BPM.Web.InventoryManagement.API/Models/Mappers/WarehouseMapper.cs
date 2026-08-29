using BPM.Web.InventoryManagement.API.Models.DTOs;
using BPM.Web.InventoryManagement.API.Models.Entities;

namespace BPM.Web.InventoryManagement.API.Models.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse ToEntity(this WarehouseCreateDto dto)
        {
            return new Warehouse
            {
                Id = Guid.NewGuid(),
                WarehouseCode = dto.WarehouseCode,
                WarehouseName = dto.WarehouseName,
                DistributorId = dto.DistributorId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Warehouse ToEntity(this WarehouseUpdateDto dto, Warehouse entity)
        {
            entity.WarehouseName = dto.WarehouseName;
            entity.DistributorId = dto.DistributorId;
            entity.AddressLine1 = dto.AddressLine1;
            entity.AddressLine2 = dto.AddressLine2;
            entity.City = dto.City;
            entity.State = dto.State;
            entity.Country = dto.Country;
            entity.PostalCode = dto.PostalCode;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ModifiedOn = DateTime.UtcNow;

            return entity;
        }

        public static WarehouseResponseDto ToDto(this Warehouse entity)
        {
            return new WarehouseResponseDto
            {
                Id = entity.Id,
                WarehouseCode = entity.WarehouseCode,
                WarehouseName = entity.WarehouseName,
                DistributorId = entity.DistributorId,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                City = entity.City,
                State = entity.State,
                Country = entity.Country,
                PostalCode = entity.PostalCode,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static IEnumerable<WarehouseResponseDto> ToDtoList(this IEnumerable<Warehouse> entities)
        {
            return entities.Select(ToDto);
        }
    }
}
