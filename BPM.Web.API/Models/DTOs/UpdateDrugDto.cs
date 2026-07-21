using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
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