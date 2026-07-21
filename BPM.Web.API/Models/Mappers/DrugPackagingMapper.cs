using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DrugPackagingMapper
    {
        public static DrugPackaging ToEntity(this CreateDrugPackagingDto dto)
        {
            return new DrugPackaging
            {
                DrugId = dto.DrugId,
                PackageUomId = dto.PackageUomId,
                ContainsUomId = dto.ContainsUomId,
                Quantity = dto.Quantity,
                TotalUnits = dto.TotalUnits,
                UnitPrice = dto.UnitPrice,
                PackagePrice = dto.PackagePrice,
                Barcode = dto.Barcode,
                GrossWeight = dto.GrossWeight,
                NetWeight = dto.NetWeight,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static DrugPackaging ToEntity(this UpdateDrugPackagingDto dto)
        {
            return new DrugPackaging
            {
                PackagingId = dto.PackagingId,
                DrugId = dto.DrugId,
                PackageUomId = dto.PackageUomId,
                ContainsUomId = dto.ContainsUomId,
                Quantity = dto.Quantity,
                TotalUnits = dto.TotalUnits,
                UnitPrice = dto.UnitPrice,
                PackagePrice = dto.PackagePrice,
                Barcode = dto.Barcode,
                GrossWeight = dto.GrossWeight,
                NetWeight = dto.NetWeight,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                IsActive = dto.IsActive
            };
        }

        public static DrugPackagingDto ToDto(this DrugPackaging entity)
        {
            return new DrugPackagingDto
            {
                PackagingId = entity.PackagingId,
                DrugId = entity.DrugId,
                PackageUomId = entity.PackageUomId,
                ContainsUomId = entity.ContainsUomId,
                Quantity = entity.Quantity,
                TotalUnits = entity.TotalUnits,
                UnitPrice = entity.UnitPrice,
                PackagePrice = entity.PackagePrice,
                Barcode = entity.Barcode,
                GrossWeight = entity.GrossWeight,
                NetWeight = entity.NetWeight,
                Length = entity.Length,
                Width = entity.Width,
                Height = entity.Height,
                IsActive = entity.IsActive,
                CreatedOn = entity.CreatedOn,
                DrugName = entity.Drug?.DrugName,
                DrugCode = entity.Drug?.DrugCode,
                PackageUomName = entity.PackageUom?.UomName,
                PackageUomCode = entity.PackageUom?.UomCode,
                ContainsUomName = entity.ContainsUom?.UomName,
                ContainsUomCode = entity.ContainsUom?.UomCode
            };
        }

        public static List<DrugPackagingDto> ToDtoList(this IEnumerable<DrugPackaging> entities)
        {
            return entities.Select(ToDto).ToList();
        }

        public static void UpdateEntity(this UpdateDrugPackagingDto dto, DrugPackaging entity)
        {
            entity.DrugId = dto.DrugId;
            entity.PackageUomId = dto.PackageUomId;
            entity.ContainsUomId = dto.ContainsUomId;
            entity.Quantity = dto.Quantity;
            entity.TotalUnits = dto.TotalUnits;
            entity.UnitPrice = dto.UnitPrice;
            entity.PackagePrice = dto.PackagePrice;
            entity.Barcode = dto.Barcode;
            entity.GrossWeight = dto.GrossWeight;
            entity.NetWeight = dto.NetWeight;
            entity.Length = dto.Length;
            entity.Width = dto.Width;
            entity.Height = dto.Height;
            entity.IsActive = dto.IsActive;
        }
    }
}
