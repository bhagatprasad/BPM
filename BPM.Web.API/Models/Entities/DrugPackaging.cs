using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("drug_packaging")]
    public class DrugPackaging
    {
        [Key]
        [Column("packagingid")]
        public Guid PackagingId { get; set; }

        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Column("package_uomid")]
        public Guid PackageUomId { get; set; }

        [Column("contains_uomid")]
        public Guid ContainsUomId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("total_units")]
        public int TotalUnits { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("package_price")]
        public decimal PackagePrice { get; set; }

        [Column("barcode")]
        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Column("gross_weight")]
        public decimal? GrossWeight { get; set; }

        [Column("net_weight")]
        public decimal? NetWeight { get; set; }

        [Column("length")]
        public decimal? Length { get; set; }

        [Column("width")]
        public decimal? Width { get; set; }

        [Column("height")]
        public decimal? Height { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey(nameof(DrugId))]
        public virtual Drug? Drug { get; set; }

        [ForeignKey(nameof(PackageUomId))]
        public virtual DrugUom? PackageUom { get; set; }

        [ForeignKey(nameof(ContainsUomId))]
        public virtual DrugUom? ContainsUom { get; set; }
    }
}

