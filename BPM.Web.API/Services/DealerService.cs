using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
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

        public async Task<List<DealerDto>> GetAllDealersAsync()
        {
            var dealers = await _dealerRepository.GetAllDealersAsync();

            return dealers.ToDto();
        }

        public async Task<DealerDto?> GetDealerByIdAsync(Guid dealerId)
        {
            var dealer = await _dealerRepository.GetDealerByIdAsync(dealerId);

            return dealer?.ToDto();
        }

        public async Task<bool> InsertDealerAsync(CreateDealerDto dealerDto)
        {
            var dealer = dealerDto.ToEntity();

            return await _dealerRepository.InsertDealerAsync(dealer);
        }

        public async Task<bool> UpdateDealerAsync(UpdateDealerDto dealerDto)
        {
            var existingDealer = await _dealerRepository.GetDealerByIdAsync(dealerDto.Id);

            if (existingDealer == null)
                return false;

            dealerDto.MapToEntity(existingDealer);

            return await _dealerRepository.UpdateDealerAsync(existingDealer);
        }

        public async Task<bool> DeleteDealerAsync(Guid dealerId)
        {
            return await _dealerRepository.DeleteDealerAsync(dealerId);
        }
    }
}