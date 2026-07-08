using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("drugs")]
    public class Drug
    {
        [Key]
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

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
