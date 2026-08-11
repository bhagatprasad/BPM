using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        public PurchaseOrderService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public async Task<List<PurchaseOrderResponseDto>> GetAllPurchaseOrdersAsync()
        {
           return await _repositoryFactory.SendAsync<List<PurchaseOrderResponseDto>>(HttpMethod.Get, "PurchaseOrder/FetchPurchaseOrders");
        }

        public async Task<List<PurchaseOrderResponseDto>> GetPurchaseOrdersByDealerAsync(Guid dealerId)
        {
            return await _repositoryFactory.SendAsync<List<PurchaseOrderResponseDto>>(HttpMethod.Get, $"PurchaseOrder/FetchPurchaseOrderByDealer/{dealerId}");
        }
    }
}
