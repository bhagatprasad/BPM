using BPM.Web.Orders.API.Models.DTOs;

namespace BPM.Web.Orders.API.Integrations
{
    public interface IDrugService
    {
        Task<List<ResponseDrugDto>> GetAllDrugsAsync();
    }
}
