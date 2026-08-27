using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;

namespace BPM.Web.Drug.API.Models.Mappers
{
    public static class DrugCategoryMapper
    {
        public static DrugCategory ToEntity(this DrugCategoryDto.CreateDrugCategoryDto dto)
        {
            return new DrugCategory
            {
                CategoryCode = dto.CategoryCode,
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        public static DrugCategory ToEntity(this DrugCategoryDto.UpdateDrugCategoryDto dto)
        {
            return new DrugCategory
            {
                Id = dto.Id,
                CategoryCode = dto.CategoryCode,
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                IsActive = dto.IsActive

            };
        }

        public static DrugCategoryDto.ResponseDrugCategoryDto ToDto(this DrugCategory entity)
        {
            return new DrugCategoryDto.ResponseDrugCategoryDto
            {
                Id = entity.Id,
                CategoryCode = entity.CategoryCode,
                CategoryName = entity.CategoryName,
                Description=entity.Description,
                IsActive = entity.IsActive
            };        
        }

        public static List<DrugCategoryDto.ResponseDrugCategoryDto>ToDtoList(this IEnumerable<DrugCategory> entities) 
        {
            return entities.Select(entity=>entity.ToDto()).ToList();
        }
    }
}
