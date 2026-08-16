using BPM.Web.API.Models.DTOs.StockMovement;

namespace BPM.Web.API.Services
{
    public interface IStockMovementService
    {
        Task<StockMovementResponseDto> CreateAsync(StockMovementCreateDto dto);

        Task<IEnumerable<StockMovementResponseDto>> GetAllAsync();

        Task<StockMovementResponseDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<StockMovementResponseDto>> GetByInventoryAsync(Guid inventoryId);

        Task<IEnumerable<StockMovementResponseDto>> GetByDrugAsync(Guid drugId);

        Task<IEnumerable<StockMovementResponseDto>> GetByWarehouseAsync(Guid warehouseId);

        Task<IEnumerable<StockMovementResponseDto>> GetByDistributorAsync(Guid distributorId);
    }
}
