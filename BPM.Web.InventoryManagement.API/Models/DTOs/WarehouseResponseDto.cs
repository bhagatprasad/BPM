namespace BPM.Web.InventoryManagement.API.Models.DTOs
{
    public class WarehouseResponseDto
    {
        public Guid Id { get; set; }

        public string WarehouseCode { get; set; } = string.Empty;

        public string WarehouseName { get; set; } = string.Empty;
        public Guid? DistributorId { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
