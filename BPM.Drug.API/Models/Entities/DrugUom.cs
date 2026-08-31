using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.Drug.API.Models.Entities
{
    [Table("drug_uom")]
    public class DrugUom
    {
        [Key]
        [Column("uomid")]
        public Guid UomId { get; set; }


        // DRUG
        [Required]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [ForeignKey(nameof(DrugId))]
        public Drug Drug { get; set; } = null!;


        // UOM DETAILS
        [Required]
        [MaxLength(20)]
        [Column("uom_code")]
        public string UomCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("uom_name")]
        public string UomName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        [Column("uom_type")]
        public string UomType { get; set; } = string.Empty;


        // PARENT UOM
        [Column("parent_uomid")]
        public Guid? ParentUomId { get; set; }

        [ForeignKey(nameof(ParentUomId))]
        public DrugUom? ParentUom { get; set; }


        // QUANTITY / CONVERSION
        [Column("quantity_per_parent")]
        public int? QuantityPerParent { get; set; }

        [Column("conversion_factor", TypeName = "numeric(18,4)")]
        public decimal ConversionFactor { get; set; } = 1;


        // FLAGS
        [Column("is_base_unit")]
        public bool IsBaseUnit { get; set; } = false;

        [Column("is_purchase_uom")]
        public bool IsPurchaseUom { get; set; } = false;

        [Column("is_sales_uom")]
        public bool IsSalesUom { get; set; } = true;

        [Column("is_inventory_uom")]
        public bool IsInventoryUom { get; set; } = true;


        // DISPLAY
        [Column("display_order")]
        public int DisplayOrder { get; set; } = 1;

        [MaxLength(250)]
        [Column("remarks")]
        public string? Remarks { get; set; }


        // STATUS
        [Column("isactive")]
        public bool IsActive { get; set; } = true;


        // AUDIT
        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }


        // CHILD UOMS
        public ICollection<DrugUom> ChildUoms { get; set; }
            = new List<DrugUom>();
    }
}