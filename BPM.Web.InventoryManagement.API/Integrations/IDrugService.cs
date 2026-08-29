using BPM.Web.InventoryManagement.API.Models.DTOs;

namespace BPM.Web.InventoryManagement.API.Integrations
{
    public interface IDrugService
    {
        Task<List<DrugDto>> GetAllDrugsAsync();
    }
}
