using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DrugFormMapper
    {
        public static DrugForm ToEntity(this CreateDrugFormDto dto)
        {
            return new DrugForm
            {
                FormCode = dto.FormCode.ToUpper().Trim(),
                FormName = dto.FormName.Trim(),
                FormType = dto.FormType?.Trim(),
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static DrugForm ToEntity(this UpdateDrugFormDto dto)
        {
            return new DrugForm
            {
                FormId = dto.FormId,
                FormCode = dto.FormCode.ToUpper().Trim(),
                FormName = dto.FormName.Trim(),
                FormType = dto.FormType?.Trim(),
                IsActive = dto.IsActive,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public static DrugFormDto ToDto(this DrugForm entity)
        {
            return new DrugFormDto
            {
                FormId = entity.FormId,
                FormCode = entity.FormCode,
                FormName = entity.FormName,
                FormType = entity.FormType,
                IsActive = entity.IsActive,
                CreatedOn = entity.CreatedOn,
                ModifiedOn = entity.ModifiedOn
            };
        }

        public static List<DrugFormDto> ToDtoList(this IEnumerable<DrugForm> entities)
        {
            return entities.Select(ToDto).ToList();
        }

        public static void UpdateEntity(this UpdateDrugFormDto dto, DrugForm entity)
        {
            entity.FormCode = dto.FormCode.ToUpper().Trim();
            entity.FormName = dto.FormName.Trim();
            entity.FormType = dto.FormType?.Trim();
            entity.IsActive = dto.IsActive;
            entity.ModifiedOn = DateTime.UtcNow;
        }
    }
}
