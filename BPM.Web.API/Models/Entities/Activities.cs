using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.Entities
{
    [Table("activities")]
    public class Activities
    {
        [Key]
        [Column("activityid")]
        public Guid ActivityId { get; set; }

        [Required]
        [Column("activityname")]
        [StringLength(100)]
        public string ActivityName { get; set; } = string.Empty;

        [Required]
        [Column("code")]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

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
