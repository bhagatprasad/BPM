using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugMapper
    {
        public static BPM.Web.Drug.API.Models.Entities.Drug ToEntity(this CreateDrugDto dto)
        {
            return new BPM.Web.Drug.API.Models.Entities.Drug
            {
                DrugCode = dto.DrugCode,
                DrugName = dto.DrugName,
                GenericName = dto.GenericName,
                BrandName = dto.BrandName,
                Manufacturer = dto.Manufacturer,
                Category = dto.Category,
                HsnCode = dto.HSNCode,
                ScheduleType = dto.ScheduleType,
                Packing = dto.Packing,
                Strength = dto.Strength,
            };
        }

        public static BPM.Web.Drug.API.Models.Entities.Drug ToEntity(this UpdateDrugDto dto)
        {
            return new BPM.Web.Drug.API.Models.Entities.Drug
            {
                DrugId = dto.DrugId,
                DrugCode = dto.DrugCode,
                DrugName = dto.DrugName,
                GenericName = dto.GenericName,
                BrandName = dto.BrandName,
                Manufacturer = dto.Manufacturer,
                Category = dto.Category,
                HsnCode = dto.HSNCode,
                ScheduleType = dto.ScheduleType,
                Packing = dto.Packing,
                Strength = dto.Strength,
                IsActive = dto.IsActive
            };
        }

        public static ResponseDrugDto ToDto(this BPM.Web.Drug.API.Models.Entities.Drug entity)
        {
            return new ResponseDrugDto
            {
                DrugId = entity.DrugId,
                DrugCode = entity.DrugCode,
                DrugName = entity.DrugName,
                GenericName = entity.GenericName,
                BrandName = entity.BrandName,
                Manufacturer = entity.Manufacturer,
                Category = entity.Category,
                HSNCode = entity.HsnCode,
                ScheduleType = entity.ScheduleType,
                Packing = entity.Packing,
                Strength = entity.Strength,
                IsActive = entity.IsActive,
                DrugUoms = entity.DrugUoms != null
                    ? ToDtoList(entity.DrugUoms.ToList())
                    : new List<DrugUomDto>(),

                DrugPackagings = entity.DrugPackagings != null
                    ? ToDtoList(entity.DrugPackagings.ToList())
                    : new List<DrugPackagingDto>()
            };
        }

        public static List<ResponseDrugDto> ToDtoList(this IEnumerable<BPM.Web.Drug.API.Models.Entities.Drug> entities)
        {
            return entities.Select(entity => entity.ToDto()).ToList();
        }

        public static DrugUomDto ToDto(this DrugUom entity)
        {
            return new DrugUomDto
            {
                UomId = entity.UomId,
                DrugId = entity.DrugId,
                UomCode = entity.UomCode,
                UomName = entity.UomName,
                UomType = entity.UomType,
                ParentUomId = entity.ParentUomId,
                QuantityPerParent = entity.QuantityPerParent,
                ConversionFactor = entity.ConversionFactor,
                IsBaseUnit = entity.IsBaseUnit,
                IsPurchaseUom = entity.IsPurchaseUom,
                IsSalesUom = entity.IsSalesUom,
                IsInventoryUom = entity.IsInventoryUom,
                DisplayOrder = entity.DisplayOrder,
                Remarks = entity.Remarks,
                IsActive = entity.IsActive,
                CreatedOn = entity.CreatedOn,
                ModifiedOn = entity.ModifiedOn,
                DrugName = entity.Drug?.DrugName,
                ParentUomName = entity.ParentUom?.UomName
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
        public static List<DrugUomDto> ToDtoList(this IEnumerable<DrugUom> entities)
        {
            return entities.Select(ToDto).ToList();
        }
    }
}
