namespace BPM.Web.API.Models.DTOs.Discount
{
    public class SupplierDiscountResponseDto
    {
        public Guid Id { get; set; }

        public Guid SupplierId { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
