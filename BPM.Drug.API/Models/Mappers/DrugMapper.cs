using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugMapper
    {
        public static DrugEntity ToEntity(this DrugDto.CreateDrugDto dto)
        {
            return new DrugEntity
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
                Strength = dto.Strength
            };
        }

        public static DrugEntity ToEntity(this DrugDto.UpdateDrugDto dto)
        {
            return new DrugEntity
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

        public static DrugDto.ResponseDrugDto ToDto(this DrugEntity entity)
        {
            return new DrugDto.ResponseDrugDto
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
                IsActive = entity.IsActive
            };
        }

        public static List<DrugDto.ResponseDrugDto> ToDtoList(this IEnumerable<DrugEntity> entities)
        {
            return entities.Select(entity => entity.ToDto()).ToList();
        }
    }
}
