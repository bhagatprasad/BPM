namespace BPM.Web.API.Models.DTOs.Discount
{
    public class VolumeDiscountTierResponseDto
    {
        public Guid Id { get; set; }

        public Guid SupplierId { get; set; }

        public int MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }

        public decimal DiscountPercentage { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
