namespace BPM.Web.Orders.API.Models.DTOs
{
    public class ProcessSalesOrderDto
    {
        public Guid SalesOrderId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
