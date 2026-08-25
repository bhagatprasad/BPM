using BPM.Web.Operations.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public PurchaseOrderService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersAllAsync()
        {
            return await _repositoryFactory.SendAsync<List<PurchaseOrderResponseDto>>(HttpMethod.Get, "PurchaseOrder/get-purchase-orders");
        }
    }
}
