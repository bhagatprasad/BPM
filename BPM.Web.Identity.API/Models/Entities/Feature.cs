using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.Identity.API.Models.Entities
{
    [Table("feature")]
    public class Feature
    {
        [Key]
        [Column("featureid")]
        public Guid FeatureId { get; set; }

        [Required]
        [Column("featurename")]
        [MaxLength(100)]
        public string FeatureName { get; set; }

        [Required]
        [Column("code")]
        [MaxLength(20)]
        public string Code { get; set; }

        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
    }
}
