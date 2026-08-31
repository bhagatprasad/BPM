using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugPackagingMapper
    {
        // =========================
        // CREATE DTO → ENTITY
        // =========================

        public static DrugPackaging ToEntity(this DrugPackagingDto.CreateDrugPackagingDto dto)
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

                IsActive = true,

                CreatedOn = DateTime.UtcNow
            };
        }


        // =========================
        // UPDATE DTO → ENTITY
        // =========================

        public static DrugPackaging ToEntity(this DrugPackagingDto.UpdateDrugPackagingDto dto)
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


        // =========================
        // ENTITY → RESPONSE DTO
        // =========================

        public static DrugPackagingDto.ResponseDrugPackagingDto ToDto(this DrugPackaging entity)
        {
            return new DrugPackagingDto.ResponseDrugPackagingDto
            {
                PackagingId = entity.PackagingId,


                // DRUG
                DrugId = entity.DrugId,

                DrugCode = entity.Drug?.DrugCode,
                DrugName = entity.Drug?.DrugName,


                // PACKAGE UOM
                PackageUomId = entity.PackageUomId,

                PackageUomCode = entity.PackageUom?.UomCode,
                PackageUomName = entity.PackageUom?.UomName,


                // CONTAINS UOM
                ContainsUomId = entity.ContainsUomId,

                ContainsUomCode = entity.ContainsUom?.UomCode,
                ContainsUomName = entity.ContainsUom?.UomName,


                // QUANTITY
                Quantity = entity.Quantity,
                TotalUnits = entity.TotalUnits,


                // PRICING
                UnitPrice = entity.UnitPrice,
                PackagePrice = entity.PackagePrice,


                // BARCODE
                Barcode = entity.Barcode,


                // WEIGHT
                GrossWeight = entity.GrossWeight,
                NetWeight = entity.NetWeight,


                // DIMENSIONS
                Length = entity.Length,
                Width = entity.Width,
                Height = entity.Height,


                // STATUS
                IsActive = entity.IsActive,


                // AUDIT
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }


        // =========================
        // ENTITY LIST → DTO LIST
        // =========================

        public static List<DrugPackagingDto.ResponseDrugPackagingDto> ToDtoList(this IEnumerable<DrugPackaging> entities)
        {
            return entities.Select(x => x.ToDto()).ToList();
        }
    }
}
