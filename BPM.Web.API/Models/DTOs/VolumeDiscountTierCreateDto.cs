using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs.Discount
{
    public class VolumeDiscountTierCreateDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Range(1, int.MaxValue)]
        public int MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }

        [Range(0, 50)]
        public decimal DiscountPercentage { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
