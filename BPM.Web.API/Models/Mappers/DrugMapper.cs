using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DrugMapper
    {
        public static Drug ToEntity(CreateDrugDto dto)
        {
            return new Drug
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

        public static Drug ToEntity(UpdateDrugDto dto)
        {
            return new Drug
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

        public static DrugDto ToDto(Drug entity)
        {
            return new DrugDto
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
                    ? DrugUomMapper.ToDtoList(entity.DrugUoms.ToList())
                    : new List<DrugUomDto>(),

                DrugPackagings = entity.DrugPackagings != null
                    ? DrugPackagingMapper.ToDtoList(entity.DrugPackagings.ToList())
                    : new List<DrugPackagingDto>()
            };
        }

        public static List<DrugDto> ToDtoList(List<Drug> entities)
        {
            return entities.Select(ToDto).ToList();
        }
    }
}