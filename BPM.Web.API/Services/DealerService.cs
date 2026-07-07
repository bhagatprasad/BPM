using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class DealerService : IDealerService
    {
        private readonly IDealerRepository _dealerRepository;

        public DealerService(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        public async Task<List<Dealer>> GetAllDealersAsync()
        {
            return await _dealerRepository.GetAllDealersAsync();
        }

        public async Task<Dealer?> GetDealerByIdAsync(Guid dealerId)
        {
            return await _dealerRepository.GetDealerByIdAsync(dealerId);
        }

        public async Task<bool> InsertDealerAsync(Dealer dealer)
        {
            return await _dealerRepository.InsertDealerAsync(dealer);
        }

        public async Task<bool> UpdateDealerAsync(Dealer dealer)
        {
            return await _dealerRepository.UpdateDealerAsync(dealer);
        }

        public async Task<bool> DeleteDealerAsync(Guid dealerId)
        {
            return await _dealerRepository.DeleteDealerAsync(dealerId);
        }
    }
}
