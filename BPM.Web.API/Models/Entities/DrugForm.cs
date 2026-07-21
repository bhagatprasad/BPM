using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("drug_forms")]
    public class DrugForm
    {
        [Key]
        [Column("formid")]
        public Guid FormId { get; set; }

        [Column("formcode")]
        [MaxLength(20)]
        public string FormCode { get; set; } = string.Empty;

        [Column("formname")]
        [MaxLength(100)]
        public string FormName { get; set; } = string.Empty;

        [Column("formtype")]
        [MaxLength(50)]
        public string? FormType { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

    }
}
