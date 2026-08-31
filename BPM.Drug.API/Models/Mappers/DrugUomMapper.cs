using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugUomMapper
    {
        // =========================
        // CREATE DTO → ENTITY
        // =========================

        public static DrugUom ToEntity(
            this DrugUomDto.CreateDrugUomDto dto)
        {
            return new DrugUom
            {
                DrugId = dto.DrugId,

                UomCode = dto.UomCode,
                UomName = dto.UomName,
                UomType = dto.UomType,

                ParentUomId = dto.ParentUomId,

                QuantityPerParent = dto.QuantityPerParent,
                ConversionFactor = dto.ConversionFactor,

                IsBaseUnit = dto.IsBaseUnit,
                IsPurchaseUom = dto.IsPurchaseUom,
                IsSalesUom = dto.IsSalesUom,
                IsInventoryUom = dto.IsInventoryUom,

                DisplayOrder = dto.DisplayOrder,
                Remarks = dto.Remarks,

                IsActive = true,

                CreatedOn = DateTime.UtcNow
            };
        }


        // =========================
        // UPDATE DTO → ENTITY
        // =========================

        public static DrugUom ToEntity(
            this DrugUomDto.UpdateDrugUomDto dto)
        {
            return new DrugUom
            {
                UomId = dto.UomId,

                DrugId = dto.DrugId,

                UomCode = dto.UomCode,
                UomName = dto.UomName,
                UomType = dto.UomType,

                ParentUomId = dto.ParentUomId,

                QuantityPerParent = dto.QuantityPerParent,
                ConversionFactor = dto.ConversionFactor,

                IsBaseUnit = dto.IsBaseUnit,
                IsPurchaseUom = dto.IsPurchaseUom,
                IsSalesUom = dto.IsSalesUom,
                IsInventoryUom = dto.IsInventoryUom,

                DisplayOrder = dto.DisplayOrder,
                Remarks = dto.Remarks,

                IsActive = dto.IsActive,

                ModifiedOn = DateTime.UtcNow
            };
        }


        // =========================
        // ENTITY → RESPONSE DTO
        // =========================

        public static DrugUomDto.ResponseDrugUomDto ToDto(
            this DrugUom entity)
        {
            return new DrugUomDto.ResponseDrugUomDto
            {
                UomId = entity.UomId,

                DrugId = entity.DrugId,

                DrugCode = entity.Drug?.DrugCode,
                DrugName = entity.Drug?.DrugName,


                // UOM DETAILS
                UomCode = entity.UomCode,
                UomName = entity.UomName,
                UomType = entity.UomType,


                // PARENT UOM
                ParentUomId = entity.ParentUomId,

                ParentUomCode = entity.ParentUom?.UomCode,
                ParentUomName = entity.ParentUom?.UomName,


                // QUANTITY / CONVERSION
                QuantityPerParent = entity.QuantityPerParent,
                ConversionFactor = entity.ConversionFactor,


                // FLAGS
                IsBaseUnit = entity.IsBaseUnit,
                IsPurchaseUom = entity.IsPurchaseUom,
                IsSalesUom = entity.IsSalesUom,
                IsInventoryUom = entity.IsInventoryUom,


                // DISPLAY
                DisplayOrder = entity.DisplayOrder,
                Remarks = entity.Remarks,


                // STATUS
                IsActive = entity.IsActive,


                // AUDIT
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,

                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };
        }


        // =========================
        // ENTITY LIST → DTO LIST
        // =========================

        public static List<DrugUomDto.ResponseDrugUomDto> ToDtoList(
            this IEnumerable<DrugUom> entities)
        {
            return entities
                .Select(x => x.ToDto())
                .ToList();
        }
    }
}
