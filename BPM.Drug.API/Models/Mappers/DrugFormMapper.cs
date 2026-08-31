using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugFormMapper
    {

        // CREATE DTO → ENTITY
        public static DrugFormEntity ToEntity(this DrugFormDto.CreateDrugFormDto dto)
        {
            return new DrugFormEntity
            {
                FormCode = dto.FormCode.ToUpper().Trim(),
                FormName = dto.FormName.Trim(),
                FormType = dto.FormType?.Trim(),
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };
        }


        // UPDATE DTO → ENTITY
        public static DrugFormEntity ToEntity(this DrugFormDto.UpdateDrugFormDto dto)
        {
            return new DrugFormEntity
            {
                FormId = dto.FormId,
                FormCode = dto.FormCode.ToUpper().Trim(),
                FormName = dto.FormName.Trim(),
                FormType = dto.FormType?.Trim(),
                IsActive = dto.IsActive,
                ModifiedOn = DateTime.UtcNow
            };
        }


        // ENTITY → RESPONSE DTO
        public static DrugFormDto.ResponseDrugFormDto ToDto(this DrugFormEntity entity)
        {
            return new DrugFormDto.ResponseDrugFormDto
            {
                FormId = entity.FormId,
                FormCode = entity.FormCode,
                FormName = entity.FormName,
                FormType = entity.FormType,
                IsActive = entity.IsActive,
                CreatedOn = entity.CreatedOn,
                ModifiedOn = entity.ModifiedOn,
                DrugCount = entity.Drugs?.Count ?? 0
            };
        }


        // ENTITY LIST → RESPONSE DTO LIST
        public static List<DrugFormDto.ResponseDrugFormDto> ToDtoList(this IEnumerable<DrugFormEntity> entities)
        {
            return entities.Select(entity => entity.ToDto()).ToList();
        }


        // UPDATE EXISTING ENTITY
        public static void UpdateEntity(this DrugFormDto.UpdateDrugFormDto dto,DrugFormEntity entity)
        {
            entity.FormCode = dto.FormCode.ToUpper().Trim();
            entity.FormName = dto.FormName.Trim();
            entity.FormType = dto.FormType?.Trim();
            entity.IsActive = dto.IsActive;
            entity.ModifiedOn = DateTime.UtcNow;
        }
    }
}
