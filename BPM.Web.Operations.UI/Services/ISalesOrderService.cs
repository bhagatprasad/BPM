using BPM.Web.Operations.UI.Models;

namespace BPM.Web.Operations.UI.Services
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync();
    }
}
