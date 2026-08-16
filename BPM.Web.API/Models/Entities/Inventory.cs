using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("inventory")]
    public class Inventory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("drugid")]
        public Guid DrugId { get; set; }

        [Required]
        [Column("packagingid")]
        public Guid PackagingId { get; set; }

        [Required]
        [Column("batchid")]
        public Guid BatchId { get; set; }

        [Required]
        [Column("warehouseid")]
        public Guid WarehouseId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("reservedquantity")]
        public int ReservedQuantity { get; set; }

        [Required]
        [Column("availablequantity")]
        public int AvailableQuantity { get; set; }

        [Required]
        [Column("reorderlevel")]
        public int ReorderLevel { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        [Column("distributorid")]
        public Guid? DistributorId { get; set; }
    }
}
