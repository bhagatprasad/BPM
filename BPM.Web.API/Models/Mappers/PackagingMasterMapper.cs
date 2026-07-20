using BPM.Web.API.Models.DTOs.Packaging;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class PackagingMasterMapper
    {
        public static PackagingMaster ToEntity(CreatePackagingMasterDto dto)
        {
            return new PackagingMaster
            {
                PackagingCode = dto.PackagingCode,
                PackagingName = dto.PackagingName,
                Description = dto.Description,
                ContainsQuantity = dto.ContainsQuantity,
                UomId = dto.UomId
            };
        }

        public static PackagingMaster ToEntity(UpdatePackagingMasterDto dto)
        {
            return new PackagingMaster
            {
                PackagingId = dto.PackagingId,
                PackagingCode = dto.PackagingCode,
                PackagingName = dto.PackagingName,
                Description = dto.Description,
                ContainsQuantity = dto.ContainsQuantity,
                UomId = dto.UomId,
                IsActive = dto.IsActive
            };
        }

        public static PackagingMasterDto ToDto(PackagingMaster entity)
        {
            return new PackagingMasterDto
            {
                PackagingId = entity.PackagingId,
                PackagingCode = entity.PackagingCode,
                PackagingName = entity.PackagingName,
                Description = entity.Description,
                ContainsQuantity = entity.ContainsQuantity,
                UomId = entity.UomId,
                IsActive = entity.IsActive
            };
        }

        public static List<PackagingMasterDto> ToDtoList(List<PackagingMaster> entities)
        {
            return entities.Select(ToDto).ToList();
        }
    }
}