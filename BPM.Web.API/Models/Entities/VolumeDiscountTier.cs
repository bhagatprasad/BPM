using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("volume_discount_tiers")]
    public class VolumeDiscountTier
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("supplierid")]
        public Guid SupplierId { get; set; }

        [Required]
        [Column("minquantity")]
        public int MinQuantity { get; set; }

        [Column("maxquantity")]
        public int? MaxQuantity { get; set; }

        [Required]
        [Column("discountpercentage", TypeName = "numeric(5,2)")]
        public decimal DiscountPercentage { get; set; }

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
