using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        public SalesOrderService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public async Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync()
        {
            return await _repositoryFactory.SendAsync<List<SalesOrderDto>>(HttpMethod.Get, "salesorder/get-sales-orders");
        }
    }
}
