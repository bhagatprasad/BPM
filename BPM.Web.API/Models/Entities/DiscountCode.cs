using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("discount_codes")]
    public class DiscountCode
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("discountcode")]
        [MaxLength(50)]
        public string DiscountCodeValue { get; set; } = string.Empty;

        [Required]
        [Column("discountpercentage", TypeName = "numeric(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column("supplierid")]
        public Guid? SupplierId { get; set; }

        [Required]
        [Column("startdate")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("expirydate")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Column("requiresapproval")]
        public bool RequiresApproval { get; set; } = true;

        [Required]
        [Column("isapproved")]
        public bool IsApproved { get; set; } = false;

        [Required]
        [Column("allowcombination")]
        public bool AllowCombination { get; set; } = false;

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
