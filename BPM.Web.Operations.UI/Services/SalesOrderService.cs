using BPM.Web.Operations.UI.Models;
using System.Net.Http;


namespace BPM.Web.Operations.UI.Services
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
