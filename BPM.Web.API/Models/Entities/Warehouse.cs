using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("warehouses")]
    public class Warehouse
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("warehousecode")]
        public string WarehouseCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("warehousename")]
        public string WarehouseName { get; set; } = string.Empty;

        [Column("distributorid")]
        public Guid? DistributorId { get; set; }

        [MaxLength(255)]
        [Column("addressline1")]
        public string? AddressLine1 { get; set; }

        [MaxLength(255)]
        [Column("addressline2")]
        public string? AddressLine2 { get; set; }

        [MaxLength(100)]
        [Column("city")]
        public string? City { get; set; }

        [MaxLength(100)]
        [Column("state")]
        public string? State { get; set; }

        [MaxLength(100)]
        [Column("country")]
        public string? Country { get; set; }

        [MaxLength(20)]
        [Column("postalcode")]
        public string? PostalCode { get; set; }

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
    }
}