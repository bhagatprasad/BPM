using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IDealerService
    {
        Task<List<DealerDto>> GetAllDealersAsync();

        Task<DealerDto?> GetDealerByIdAsync(Guid dealerId);

        Task<bool> InsertDealerAsync(CreateDealerDto dealer);

        Task<bool> UpdateDealerAsync(UpdateDealerDto dealer);

        Task<bool> DeleteDealerAsync(Guid dealerId);
    }
}
