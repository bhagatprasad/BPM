using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BPM.Web.API.Models.Entities
{
    [Table("suppliers")]
    public class Supplier
    {
        [Key]
        [Column("supplierid")]
        public Guid SupplierId { get; set; }

        [Column("suppliercode")]
        [Required]
        [MaxLength(50)]
        public string SupplierCode { get; set; } = string.Empty;

        [Column("suppliername")]
        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [Column("contactperson")]
        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        [Column("email")]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Column("phone")]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Column("alternatephone")]
        [MaxLength(20)]
        public string? AlternatePhone { get; set; }

        [Column("gstnumber")]
        [MaxLength(20)]
        public string? GSTNumber { get; set; }

        [Column("druglicensenumber")]
        [MaxLength(100)]
        public string? DrugLicenseNumber { get; set; }

        [Column("addressline1")]
        [MaxLength(200)]
        public string? AddressLine1 { get; set; }

        [Column("addressline2")]
        [MaxLength(200)]
        public string? AddressLine2 { get; set; }

        [Column("city")]
        [MaxLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [MaxLength(100)]
        public string? State { get; set; }

        [Column("country")]
        [MaxLength(100)]
        public string? Country { get; set; }

        [Column("postalcode")]
        [MaxLength(10)]
        public string? PostalCode { get; set; }

        [Column("website")]
        [MaxLength(200)]
        public string? Website { get; set; }

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
