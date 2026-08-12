namespace BPM.Web.API.Models.DTOs.Invoice
{
    public class CreateInvoiceDto
    {
        public Guid BillingId { get; set; }

        public decimal AdjustmentAmount { get; set; }

        public string? Remarks { get; set; }

        public Guid CreatedBy { get; set; }
    }
}