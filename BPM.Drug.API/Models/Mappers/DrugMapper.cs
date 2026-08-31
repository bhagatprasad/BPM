using BPM.Web.Drug.API.Models.DTOs;

using DrugEntity = BPM.Web.Drug.API.Models.Entities.Drug;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugMapper
    {
        public static DrugEntity ToEntity(this DrugDto.CreateDrugDto dto)
        {
            return new DrugEntity
            {
                FormId = dto.FormId,
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
                FormId = dto.FormId,
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

                FormId = entity.FormId,
                FormCode = entity.DrugForm?.FormCode,
                FormName = entity.DrugForm?.FormName,

                GenericName = entity.GenericName,
                BrandName = entity.BrandName,
                Manufacturer = entity.Manufacturer,
                Category = entity.Category,
                HSNCode = entity.HsnCode,
                ScheduleType = entity.ScheduleType,
                Packing = entity.Packing,
                Strength = entity.Strength,
                IsActive = entity.IsActive,

                //DrugUoms = entity.DrugUoms != null
                //    ? entity.DrugUoms.Select(x => x.ToDto()).ToList()
                //    : new List<DrugUomDto>(),

                //DrugPackagings = entity.DrugPackagings != null
                //    ? entity.DrugPackagings.Select(x => x.ToDto()).ToList()
                //    : new List<DrugPackagingDto>()
            };
        }

        public static List<DrugDto.ResponseDrugDto> ToDtoList(
            this IEnumerable<DrugEntity> entities)
        {
            return entities.Select(x => x.ToDto()).ToList();
        }

        // Rest of your DrugUom and DrugPackaging methods remain the same
    }
}