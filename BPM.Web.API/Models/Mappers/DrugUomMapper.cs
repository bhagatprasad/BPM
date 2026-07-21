using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DrugUomMapper
    {
        public static DrugUom ToEntity(this CreateDrugUomDto dto)
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
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static DrugUom ToEntity(this UpdateDrugUomDto dto)
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

        public static List<DrugUomDto> ToDtoList(this IEnumerable<DrugUom> entities)
        {
            return entities.Select(ToDto).ToList();
        }

        public static void UpdateEntity(this UpdateDrugUomDto dto, DrugUom entity)
        {
            entity.DrugId = dto.DrugId;
            entity.UomCode = dto.UomCode;
            entity.UomName = dto.UomName;
            entity.UomType = dto.UomType;
            entity.ParentUomId = dto.ParentUomId;
            entity.QuantityPerParent = dto.QuantityPerParent;
            entity.ConversionFactor = dto.ConversionFactor;
            entity.IsBaseUnit = dto.IsBaseUnit;
            entity.IsPurchaseUom = dto.IsPurchaseUom;
            entity.IsSalesUom = dto.IsSalesUom;
            entity.IsInventoryUom = dto.IsInventoryUom;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.Remarks = dto.Remarks;
            entity.IsActive = dto.IsActive;
            entity.ModifiedOn = DateTime.UtcNow;
        }
    }
}