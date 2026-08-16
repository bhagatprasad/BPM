using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.PurchaseOrder
{
    public class ValidateProductAvailabilityDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PackagingId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
