using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("manufacturers")]
    public class Manufacturer
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("manufacturercode")]
        public string ManufacturerCode { get; set; } = string.Empty;

        [Column("manufacturername")]
        public string ManufacturerName { get; set; } = string.Empty;

        [Column("contactperson")]
        public string? ContactPerson { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

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
