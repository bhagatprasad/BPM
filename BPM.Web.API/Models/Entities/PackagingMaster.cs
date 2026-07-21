using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("packaging_master")]
    public class PackagingMaster
    {
        [Key]
        [Column("packagingid")]
        public Guid PackagingId { get; set; }

        [Required]
        [Column("packaging_code")]
        public string PackagingCode { get; set; } = string.Empty;

        [Required]
        [Column("packaging_name")]
        public string PackagingName { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("contains_quantity")]
        public decimal? ContainsQuantity { get; set; }

        [Column("uomid")]
        public Guid? UomId { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }
    }
}