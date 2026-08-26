using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Drug.API.Models.DTOs
{
    public class DrugDto
    {

    }

    // CREATE DRUG DTO

    public class CreateDrugDto
    {
        [Required]
        public string DrugCode { get; set; } = string.Empty;

        [Required]
        public string DrugName { get; set; } = string.Empty;

        public string? GenericName { get; set; }

        public string? BrandName { get; set; }

        public string? Manufacturer { get; set; }

        public string? Category { get; set; }

        public string? HSNCode { get; set; }

        public string? ScheduleType { get; set; }

        public string? Packing { get; set; }

        public string? Strength { get; set; }
    }


    // RESPONSE  DRUG DTO

    public class ResponseDrugDto
    {
        public Guid DrugId { get; set; }

        public string DrugCode { get; set; } = string.Empty;

        public string DrugName { get; set; } = string.Empty;

        public string? GenericName { get; set; }

        public string? BrandName { get; set; }

        public string? Manufacturer { get; set; }

        public string? Category { get; set; }

        public string? HSNCode { get; set; }

        public string? ScheduleType { get; set; }

        public string? Packing { get; set; }

        public string? Strength { get; set; }

        public bool IsActive { get; set; }

        public List<DrugUomDto> DrugUoms { get; set; } = new();

        public List<DrugPackagingDto> DrugPackagings { get; set; } = new();
    }


    // UPDATE DRUG DTO

    public class UpdateDrugDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public string DrugCode { get; set; } = string.Empty;

        [Required]
        public string DrugName { get; set; } = string.Empty;

        public string? GenericName { get; set; }

        public string? BrandName { get; set; }

        public string? Manufacturer { get; set; }

        public string? Category { get; set; }

        public string? HSNCode { get; set; }

        public string? ScheduleType { get; set; }

        public string? Packing { get; set; }

        public string? Strength { get; set; }

        public bool IsActive { get; set; }
    }
}