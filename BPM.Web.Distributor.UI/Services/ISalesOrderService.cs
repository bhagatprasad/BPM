using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync();
    }
}
