using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("promotional_offers")]
    public class PromotionalOffer
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("offername")]
        [MaxLength(200)]
        public string OfferName { get; set; } = string.Empty;

        [Required]
        [Column("supplierid")]
        public Guid SupplierId { get; set; }

        [Column("drugid")]
        public Guid? DrugId { get; set; }

        [Column("packagingid")]
        public Guid? PackagingId { get; set; }

        [Required]
        [Column("discountpercentage", TypeName = "numeric(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Required]
        [Column("startdate")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("expirydate")]
        public DateTime ExpiryDate { get; set; }

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
