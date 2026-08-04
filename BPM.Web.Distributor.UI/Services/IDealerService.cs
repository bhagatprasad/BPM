using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IDealerService
    {
        Task<List<DealerDto>> GetAllDealersAsync();
    }
}