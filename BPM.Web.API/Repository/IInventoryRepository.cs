using BPM.Web.API.Models.Entities;
namespace BPM.Web.API.Repository
{
    public interface IInventoryRepository
    {
        Task<Inventory> CreateAsync(Inventory inventory);
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(Guid id);
        Task<IEnumerable<Inventory>> GetByDistributorIdAsync(Guid distributorId);
        Task<IEnumerable<Inventory>> GetByDrugIdAsync(Guid drugId);
        Task<IEnumerable<Inventory>> GetByWarehouseIdAsync(Guid warehouseId);
        Task<Inventory?> GetInventoryForAvailabilityAsync(Guid drugId, Guid packagingId, Guid batchId, Guid warehouseId);
        Task<bool> UpdateAsync(Inventory inventory);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Inventory>> OnBoardingInventoryAsync(List<Inventory> inventory);
    }
}
