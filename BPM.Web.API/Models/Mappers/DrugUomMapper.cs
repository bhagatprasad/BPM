using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.DTOs.DrugUom;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DrugUomMapper
    {
        public static DrugUom ToEntity(CreateDrugUomDto dto)
        {
            return new DrugUom
            {
                DrugId = dto.DrugId,
                UomCode = dto.UomCode,
                UomName = dto.UomName,
                UomType = dto.UomType,
                ConversionFactor = dto.ConversionFactor,
                IsBaseUnit = dto.IsBaseUnit
            };
        }

        public static DrugUom ToEntity(UpdateDrugUomDto dto)
        {
            return new DrugUom
            {
                UomId = dto.UomId,
                DrugId = dto.DrugId,
                UomCode = dto.UomCode,
                UomName = dto.UomName,
                UomType = dto.UomType,
                ConversionFactor = dto.ConversionFactor,
                IsBaseUnit = dto.IsBaseUnit,
                IsActive = dto.IsActive
            };
        }

        public static DrugUomDto ToDto(DrugUom entity)
        {
            return new DrugUomDto
            {
                UomId = entity.UomId,
                DrugId = entity.DrugId,
                UomCode = entity.UomCode,
                UomName = entity.UomName,
                UomType = entity.UomType,
                ConversionFactor = entity.ConversionFactor,
                IsBaseUnit = entity.IsBaseUnit,
                IsActive = entity.IsActive
            };
        }

        public static List<DrugUomDto> ToDtoList(List<DrugUom> entities)
        {
            return entities.Select(ToDto).ToList();
        }
    }
}