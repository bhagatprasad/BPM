namespace BPM.Web.API.Models.DTOs.Discount
{
    public class PromotionalOfferResponseDto
    {
        public Guid Id { get; set; }

        public string OfferName { get; set; } = string.Empty;

        public Guid SupplierId { get; set; }

        public Guid? DrugId { get; set; }

        public Guid? PackagingId { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool AllowCombination { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
