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

        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Column("uom_code")]
        [MaxLength(20)]
        public string UomCode { get; set; } = string.Empty;

        [Column("uom_name")]
        [MaxLength(100)]
        public string UomName { get; set; } = string.Empty;

        [Column("uom_type")]
        [MaxLength(30)]
        public string UomType { get; set; } = string.Empty;

        [Column("parent_uomid")]
        public Guid? ParentUomId { get; set; }

        [Column("quantity_per_parent")]
        public int? QuantityPerParent { get; set; }

        [Column("conversion_factor")]
        public decimal ConversionFactor { get; set; } = 1;

        [Column("is_base_unit")]
        public bool IsBaseUnit { get; set; }

        [Column("is_purchase_uom")]
        public bool IsPurchaseUom { get; set; }

        [Column("is_sales_uom")]
        public bool IsSalesUom { get; set; } = true;

        [Column("is_inventory_uom")]
        public bool IsInventoryUom { get; set; } = true;

        [Column("display_order")]
        public int DisplayOrder { get; set; } = 1;

        [Column("remarks")]
        [MaxLength(250)]
        public string? Remarks { get; set; }

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

        // Navigation Properties
        [ForeignKey(nameof(DrugId))]
        public virtual Drug? Drug { get; set; }

        [ForeignKey(nameof(ParentUomId))]
        public virtual DrugUom? ParentUom { get; set; }
    }
}