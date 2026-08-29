using BPM.Web.InventoryManagement.API.Integrations;
using BPM.Web.InventoryManagement.API.Models.Data;
using BPM.Web.InventoryManagement.API.Models.DTOs;
using BPM.Web.InventoryManagement.API.Models.Mappers;
using BPM.Web.InventoryManagement.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.InventoryManagement.API.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly IStockMovementRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StockMovementService> _logger;

        private static readonly string[] AllowedMovementTypes = { "Purchase", "Sale", "Return", "Adjustment", "Transfer" };

        public StockMovementService(IStockMovementRepository repository, ILogger<StockMovementService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<StockMovementResponseDto> CreateAsync(StockMovementCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating stock movement for InventoryId: {InventoryId}", dto.InventoryId);

                if (!AllowedMovementTypes.Contains(dto.MovementType, StringComparer.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid movement type: {MovementType}", dto.MovementType);
                    throw new ArgumentException("Invalid movement type.");
                }

                if (dto.Quantity == 0)
                {
                    _logger.LogWarning("Stock movement quantity cannot be zero.");
                    throw new ArgumentException("Quantity cannot be zero.");
                }

                if (dto.QuantityBefore < 0)
                {
                    _logger.LogWarning("QuantityBefore cannot be negative.");
                    throw new ArgumentException("QuantityBefore cannot be negative.");
                }

                if (dto.QuantityAfter < 0)
                {
                    _logger.LogWarning("QuantityAfter cannot be negative.");
                    throw new ArgumentException("QuantityAfter cannot be negative.");
                }

                var inventoryExists = await _context.Inventories.AnyAsync(x => x.Id == dto.InventoryId);
                if (!inventoryExists)
                {
                    _logger.LogWarning("Inventory not found: {InventoryId}", dto.InventoryId);
                    throw new KeyNotFoundException("Inventory not found.");
                }

                var stockMovement = dto.ToEntity();

                var createdMovement = await _repository.CreateAsync(stockMovement);

                _logger.LogInformation("Stock movement created successfully. StockMovementId: {Id}", createdMovement.Id);

                return createdMovement.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating stock movement.");
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all stock movements.");
                var movements = await _repository.GetAllAsync();
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all stock movements.");
                throw;
            }
        }

        public async Task<StockMovementResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching stock movement by Id: {Id}", id);
                var movement = await _repository.GetByIdAsync(id);

                if (movement == null)
                {
                    _logger.LogWarning("Stock movement not found: {Id}", id);
                    return null;
                }

                return movement.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movement by Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByInventoryAsync(Guid inventoryId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for InventoryId: {InventoryId}", inventoryId);
                var movements = await _repository.GetByInventoryAsync(inventoryId);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements for InventoryId: {InventoryId}", inventoryId);
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByDrugAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for DrugId: {DrugId}", drugId);
                var movements = await _repository.GetByDrugAsync(drugId);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements for DrugId: {DrugId}", drugId);
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByWarehouseAsync(Guid warehouseId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for WarehouseId: {WarehouseId}", warehouseId);
                var movements = await _repository.GetByWarehouseAsync(warehouseId);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements for WarehouseId: {WarehouseId}", warehouseId);
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByDistributorAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for DistributorId: {DistributorId}", distributorId);
                var movements = await _repository.GetByDistributorAsync(distributorId);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements for DistributorId: {DistributorId}", distributorId);
                throw;
            }
        }

        public async Task<StockMovementResponseDto?> UpdateAsync(StockMovementUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating stock movement with Id: {Id}", dto.Id);

                var existingMovement = await _repository.GetByIdAsync(dto.Id);
                if (existingMovement == null)
                {
                    _logger.LogWarning("Stock movement not found with Id: {Id}", dto.Id);
                    return null;
                }

                if (!AllowedMovementTypes.Contains(dto.MovementType, StringComparer.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid movement type: {MovementType}", dto.MovementType);
                    throw new ArgumentException("Invalid movement type.");
                }

                if (dto.Quantity == 0)
                {
                    _logger.LogWarning("Stock movement quantity cannot be zero.");
                    throw new ArgumentException("Quantity cannot be zero.");
                }

                if (dto.QuantityBefore < 0)
                {
                    _logger.LogWarning("QuantityBefore cannot be negative.");
                    throw new ArgumentException("QuantityBefore cannot be negative.");
                }

                if (dto.QuantityAfter < 0)
                {
                    _logger.LogWarning("QuantityAfter cannot be negative.");
                    throw new ArgumentException("QuantityAfter cannot be negative.");
                }

                var updatedMovement = dto.ToEntity(existingMovement);
                var result = await _repository.UpdateAsync(updatedMovement);

                if (result == null)
                {
                    _logger.LogWarning("Stock movement update failed for Id: {Id}", dto.Id);
                    return null;
                }

                _logger.LogInformation("Stock movement updated successfully. StockMovementId: {Id}", result.Id);
                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating stock movement with Id: {Id}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting stock movement with Id: {Id}", id);

                var result = await _repository.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Stock movement not found with Id: {Id}", id);
                    return false;
                }

                _logger.LogInformation("Stock movement deleted successfully. StockMovementId: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting stock movement with Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements from {StartDate} to {EndDate}", startDate, endDate);

                if (startDate > endDate)
                {
                    _logger.LogWarning("Start date cannot be greater than end date.");
                    throw new ArgumentException("Start date cannot be greater than end date.");
                }

                var movements = await _repository.GetByDateRangeAsync(startDate, endDate);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements by date range.");
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByMovementTypeAsync(string movementType)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for MovementType: {MovementType}", movementType);

                if (!AllowedMovementTypes.Contains(movementType, StringComparer.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid movement type: {MovementType}", movementType);
                    throw new ArgumentException("Invalid movement type.");
                }

                var movements = await _repository.GetByMovementTypeAsync(movementType);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements for MovementType: {MovementType}", movementType);
                throw;
            }
        }

        public async Task<decimal> GetTotalQuantityByInventoryAsync(Guid inventoryId)
        {
            try
            {
                _logger.LogInformation("Calculating total quantity for InventoryId: {InventoryId}", inventoryId);

                var inventoryExists = await _context.Inventories.AnyAsync(x => x.Id == inventoryId);
                if (!inventoryExists)
                {
                    _logger.LogWarning("Inventory not found: {InventoryId}", inventoryId);
                    throw new KeyNotFoundException("Inventory not found.");
                }

                var totalQuantity = await _repository.GetTotalQuantityByInventoryAsync(inventoryId);

                _logger.LogInformation("Total quantity for InventoryId {InventoryId} is {TotalQuantity}", inventoryId, totalQuantity);
                return totalQuantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating total quantity for InventoryId: {InventoryId}", inventoryId);
                throw;
            }
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByInventoryAndDateRangeAsync(Guid inventoryId, DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Fetching stock movements for InventoryId: {InventoryId} from {StartDate} to {EndDate}", inventoryId, startDate, endDate);

                if (startDate > endDate)
                {
                    _logger.LogWarning("Start date cannot be greater than end date.");
                    throw new ArgumentException("Start date cannot be greater than end date.");
                }

                var inventoryExists = await _context.Inventories.AnyAsync(x => x.Id == inventoryId);
                if (!inventoryExists)
                {
                    _logger.LogWarning("Inventory not found: {InventoryId}", inventoryId);
                    throw new KeyNotFoundException("Inventory not found.");
                }

                var movements = await _repository.GetByInventoryAndDateRangeAsync(inventoryId, startDate, endDate);
                return movements.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock movements for InventoryId: {InventoryId} by date range.", inventoryId);
                throw;
            }
        }
    }
}