using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class DealerService : IDealerService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public DealerService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<DealerDto>> GetAllDealersAsync()
        {
            return await _repositoryFactory.SendAsync<List<DealerDto>>
            (
                HttpMethod.Get,
                "Dealer/getalldealers"
            );
        }
    }
}