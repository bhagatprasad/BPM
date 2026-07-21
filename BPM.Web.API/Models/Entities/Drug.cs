using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("drug")]
    public class Drug
    {
        [Key]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Required]
        [Column("drugcode")]
        public string DrugCode { get; set; } = string.Empty;

        [Required]
        [Column("drugname")]
        public string DrugName { get; set; } = string.Empty;

        [Column("genericname")]
        public string? GenericName { get; set; }

        [Column("brandname")]
        public string? BrandName { get; set; }

        [Column("manufacturer")]
        public string? Manufacturer { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("hsncode")]
        public string? HsnCode { get; set; }

        [Column("scheduletype")]
        public string? ScheduleType { get; set; }

        [Column("packing")]
        public string? Packing { get; set; }

        [Column("strength")]
        public string? Strength { get; set; }

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

        [Column("imageurl")]
        public string? ImageUrl { get; set; }

        // Navigation Properties
        public virtual ICollection<DrugUom> DrugUoms { get; set; }
            = new List<DrugUom>();

        public virtual ICollection<DrugPackaging> DrugPackagings { get; set; }
            = new List<DrugPackaging>();
    }
}