using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.Packaging
{
    public class UpdatePackagingMasterDto
    {
        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        public string PackagingCode { get; set; } = string.Empty;

        [Required]
        public string PackagingName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal? ContainsQuantity { get; set; }

        public Guid? UomId { get; set; }

        public bool IsActive { get; set; }
    }
}