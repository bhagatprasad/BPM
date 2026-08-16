using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
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
    }
}