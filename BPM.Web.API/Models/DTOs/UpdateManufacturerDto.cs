namespace BPM.Web.API.Models.DTOs
{
    public class UpdateManufacturerDto
    {
        public Guid Id { get; set; }

        public string ManufacturerCode { get; set; } = string.Empty;

        public string ManufacturerName { get; set; } = string.Empty;

        public string? ContactPerson { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? Website { get; set; }

        public bool IsActive { get; set; }
    }
}