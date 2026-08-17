using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("distributors")]
    public class Distributor
    {
        [Key]
        [Column("id")]
        public Guid DistributorId { get; set; }

        [Column("distributorcode")]
        public string DistributorCode { get; set; } = string.Empty;

        [Column("distributorname")]
        public string DistributorName { get; set; } = string.Empty;

        [Column("registrationnumber")]
        public string? RegistrationNumber { get; set; }

        [Column("druglicensenumber")]
        public string? DrugLicenseNumber { get; set; }

        [Column("gstnumber")]
        public string? GSTNumber { get; set; }

        [Column("contactperson")]
        public string? ContactPerson { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("alternatephone")]
        public string? AlternatePhone { get; set; }

        [Column("addressline1")]
        public string? AddressLine1 { get; set; }

        [Column("addressline2")]
        public string? AddressLine2 { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("state")]
        public string? State { get; set; }

        [Column("country")]
        public string? Country { get; set; }

        [Column("postalcode")]
        public string? PostalCode { get; set; }

        [Column("website")]
        public string? Website { get; set; }

        [Column("warehouseid")]
        public Guid? WarehouseId { get; set; }

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
    }
}
