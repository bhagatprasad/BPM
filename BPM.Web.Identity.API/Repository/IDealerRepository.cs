using BPM.Web.Identity.API.Models.Entities;

namespace BPM.Web.Identity.API.Repository
{
    public interface IDealerRepository
    {
        Task<List<Dealer>> GetAllDealersAsync();

        Task<Dealer?> GetDealerByIdAsync(Guid dealerId);

        Task<bool> InsertDealerAsync(Dealer dealer);

        Task<bool> UpdateDealerAsync(Dealer dealer);

        Task<bool> DeleteDealerAsync(Guid dealerId);
    }
}
