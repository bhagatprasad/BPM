using BPM.Web.Identity.API.Models.DTOs;

namespace BPM.Web.Identity.API.Services
{
    public interface IDealerService
    {
        Task<List<DealerDto>> GetAllDealersAsync();

        Task<DealerDto?> GetDealerByIdAsync(Guid dealerId);

        Task<bool> InsertDealerAsync(CreateDealerDto dealer);

        Task<DealerDto> UpdateDealerAsync(Guid id, DealerUpdatedDto dealer);
        Task<bool> DeleteDealerAsync(Guid dealerId);
    }
}
