using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("supplier_discounts")]
    public class SupplierDiscount
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("supplierid")]
        public Guid SupplierId { get; set; }

        [Required]
        [Column("discountpercentage", TypeName = "numeric(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Required]
        [Column("validfrom")]
        public DateTime ValidFrom { get; set; }

        [Column("validto")]
        public DateTime? ValidTo { get; set; }

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
