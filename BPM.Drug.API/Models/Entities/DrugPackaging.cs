using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.Drug.API.Models.Entities
{
    [Table("drug_packaging")]
    public class DrugPackaging
    {
        [Key]
        [Column("packagingid")]
        public Guid PackagingId { get; set; }


        // DRUG
        [Required]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [ForeignKey(nameof(DrugId))]
        public Drug Drug { get; set; } = null!;


        // PACKAGE UOM
        [Required]
        [Column("package_uomid")]
        public Guid PackageUomId { get; set; }

        [ForeignKey(nameof(PackageUomId))]
        public DrugUom PackageUom { get; set; } = null!;


        // CONTAINS UOM
        [Required]
        [Column("contains_uomid")]
        public Guid ContainsUomId { get; set; }

        [ForeignKey(nameof(ContainsUomId))]
        public DrugUom ContainsUom { get; set; } = null!;


        // QUANTITY
        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }


        [Required]
        [Column("total_units")]
        public int TotalUnits { get; set; }


        // PRICING
        [Required]
        [Column("unit_price", TypeName = "numeric(18,2)")]
        public decimal UnitPrice { get; set; }


        [Required]
        [Column("package_price", TypeName = "numeric(18,2)")]
        public decimal PackagePrice { get; set; }


        // BARCODE
        [MaxLength(100)]
        [Column("barcode")]
        public string? Barcode { get; set; }


        // WEIGHT
        [Column("gross_weight", TypeName = "numeric(10,2)")]
        public decimal? GrossWeight { get; set; }

        [Column("net_weight", TypeName = "numeric(10,2)")]
        public decimal? NetWeight { get; set; }


        // DIMENSIONS
        [Column("length", TypeName = "numeric(10,2)")]
        public decimal? Length { get; set; }

        [Column("width", TypeName = "numeric(10,2)")]
        public decimal? Width { get; set; }

        [Column("height", TypeName = "numeric(10,2)")]
        public decimal? Height { get; set; }


        // STATUS
        [Column("isactive")]
        public bool IsActive { get; set; } = true;


        // AUDIT
        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}