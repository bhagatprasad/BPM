namespace BPM.Web.InventoryManagement.API.Models.DTOs
{
    public class DistributorDto
    {
        public Guid DistributorId { get; set; }
        public string DistributorCode { get; set; } = string.Empty;
        public string DistributorName { get; set; } = string.Empty;
        public string? RegistrationNumber { get; set; }
        public string? DrugLicenseNumber { get; set; }
        public string? GSTNumber { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AlternatePhone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Website { get; set; }
        public Guid? WarehouseId { get; set; }
        public bool IsActive { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
