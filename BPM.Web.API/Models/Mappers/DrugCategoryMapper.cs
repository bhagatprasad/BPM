using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DrugCategoryMapper
    {
        public static DrugCategoryDto ToDto(this DrugCategory drugCategory)
        {
            return new DrugCategoryDto
            {
                Id = drugCategory.Id,
                CategoryCode = drugCategory.CategoryCode,
                CategoryName = drugCategory.CategoryName,
                Description = drugCategory.Description,
                IsActive = drugCategory.IsActive
            };
        }

        public static List<DrugCategoryDto> ToDto(this IEnumerable<DrugCategory> drugCategories)
        {
            return drugCategories.Select(d => d.ToDto()).ToList();
        }

        public static DrugCategory ToEntity(this CreateDrugCategoryDto dto)
        {
            return new DrugCategory
            {
                Id = Guid.NewGuid(),
                CategoryCode = dto.CategoryCode,
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static DrugCategory ToEntity(this UpdateDrugCategoryDto dto)
        {
            return new DrugCategory
            {
                Id = dto.Id,
                CategoryCode = dto.CategoryCode,
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public static void MapToEntity(this UpdateDrugCategoryDto dto, DrugCategory drugCategory)
        {
            drugCategory.CategoryCode = dto.CategoryCode;
            drugCategory.CategoryName = dto.CategoryName;
            drugCategory.Description = dto.Description;
            drugCategory.IsActive = dto.IsActive;
            drugCategory.ModifiedOn = DateTime.UtcNow;
        }
    }
}
