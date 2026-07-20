namespace BPM.Web.API.Models.DTOs.Packaging
{
    public class PackagingMasterDto
    {
        public Guid PackagingId { get; set; }

        public string PackagingCode { get; set; } = string.Empty;

        public string PackagingName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal? ContainsQuantity { get; set; }

        public Guid? UomId { get; set; }

        public bool IsActive { get; set; }
    }
}