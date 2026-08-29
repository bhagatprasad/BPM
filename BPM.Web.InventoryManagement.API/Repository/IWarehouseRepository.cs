using BPM.Web.InventoryManagement.API.Models.Entities;

namespace BPM.Web.InventoryManagement.API.Repository
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> CreateAsync(Warehouse warehouse);

        Task<List<Warehouse>> GetAllAsync();

        Task<Warehouse?> GetByIdAsync(Guid id);

        Task<Warehouse?> GetByCodeAsync(string warehouseCode);

        Task<List<Warehouse>> GetByDistributorIdAsync(Guid distributorId);

        Task<bool> UpdateAsync(Warehouse warehouse);

        Task<bool> DeleteAsync(Guid id);
    }
}
