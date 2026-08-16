namespace BPM.Web.API.Models.DTOs.Discount
{
    public class DiscountCodeResponseDto
    {
        public Guid Id { get; set; }

        public string DiscountCode { get; set; } = string.Empty;

        public decimal DiscountPercentage { get; set; }

        public Guid? SupplierId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool RequiresApproval { get; set; }

        public bool IsApproved { get; set; }

        public bool AllowCombination { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
