namespace BPM.Web.API.Models.DTOs.SalesOrder
{
    public class ProcessSalesOrderDto
    {
        public Guid SalesOrderId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}