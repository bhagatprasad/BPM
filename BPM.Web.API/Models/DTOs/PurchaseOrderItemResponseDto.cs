namespace BPM.Web.API.Models.DTOs
{
    public class PurchaseOrderItemResponseDto
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Guid DrugId { get; set; }

        public string? DrugName { get; set; }

        public string? DrugCode { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public int ReceivedQuantity { get; set; }

        public int PendingQuantity { get; set; }

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? Remarks { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
