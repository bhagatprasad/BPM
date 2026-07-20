using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("drug_uom")]
    public class DrugUom
    {
        [Key]
        [Column("uomid")]
        public Guid UomId { get; set; }

        [Required]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Required]
        [Column("uom_code")]
        [MaxLength(20)]
        public string UomCode { get; set; } = string.Empty;

        [Required]
        [Column("uom_name")]
        [MaxLength(100)]
        public string UomName { get; set; } = string.Empty;

        [Required]
        [Column("uom_type")]
        [MaxLength(20)]
        public string UomType { get; set; } = string.Empty;

        [Column("conversion_factor")]
        public decimal? ConversionFactor { get; set; }

        [Column("is_base_unit")]
        public bool IsBaseUnit { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        // Navigation Property
        public Drug? Drug { get; set; }
    }
}