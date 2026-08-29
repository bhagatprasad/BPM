using BPM.Web.InventoryManagement.API.Models.Entities;

namespace BPM.Web.InventoryManagement.API.Repository
{
    public interface IStockMovementRepository
    {
        Task<StockMovement> CreateAsync(StockMovement stockMovement);

        Task<IEnumerable<StockMovement>> GetAllAsync();

        Task<StockMovement?> GetByIdAsync(Guid id);

        Task<IEnumerable<StockMovement>> GetByInventoryAsync(Guid inventoryId);

        Task<IEnumerable<StockMovement>> GetByDrugAsync(Guid drugId);

        Task<IEnumerable<StockMovement>> GetByWarehouseAsync(Guid warehouseId);

        Task<IEnumerable<StockMovement>> GetByDistributorAsync(Guid distributorId);

        Task<StockMovement?> UpdateAsync(StockMovement stockMovement);

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<StockMovement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<StockMovement>> GetByMovementTypeAsync(string movementType);

        Task<decimal> GetTotalQuantityByInventoryAsync(Guid inventoryId);

        Task<IEnumerable<StockMovement>> GetByInventoryAndDateRangeAsync(Guid inventoryId, DateTime startDate, DateTime endDate);
    }
}