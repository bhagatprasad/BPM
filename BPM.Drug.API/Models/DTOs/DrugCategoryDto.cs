using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Drug.API.Models.DTOs
{
    public class DrugCategoryDto
    {
        // CREATE DRUG CATEGORY DTO

        public class CreateDrugCategoryDto
        {
            [Required]
            public string CategoryCode { get; set; } = string.Empty;

            [Required]
            public string CategoryName { get; set; } = string.Empty;

            public string? Description { get; set; }

            public bool IsActive { get; set; } = true;
        }


        // RESPONSE DRUG CATEGORY DTO

        public class ResponseDrugCategoryDto
        {
            public Guid Id { get; set; }

            public string CategoryCode { get; set; } = string.Empty;

            public string CategoryName { get; set; } = string.Empty;

            public string? Description { get; set; }

            public bool IsActive { get; set; }
        }


        // UPDATE DRUG CATEGORY DTO

        public class UpdateDrugCategoryDto
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            public string CategoryCode { get; set; } = string.Empty;

            [Required]
            public string CategoryName { get; set; } = string.Empty;

            public string? Description { get; set; }

            public bool IsActive { get; set; }
        }
    }
}
