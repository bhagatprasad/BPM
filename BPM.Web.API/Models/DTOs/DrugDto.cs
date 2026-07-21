using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Models.DTOs
{
    public class DrugDto
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
}
