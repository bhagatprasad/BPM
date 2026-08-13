namespace BPM.Web.API.Models.DTOs.Billing
{
    public class CreateBillingDto
    {
        public Guid SalesOrderId { get; set; }

        public decimal AdjustmentAmount { get; set; }

        public string? Remarks { get; set; }

        public Guid CreatedBy { get; set; }
    }
}