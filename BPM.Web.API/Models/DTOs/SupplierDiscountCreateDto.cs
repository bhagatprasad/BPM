using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.Discount
{
    public class SupplierDiscountCreateDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Range(0, 50)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}