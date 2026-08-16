using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repositories.Interfaces;
using BPM.Web.API.Services.Interfaces;

namespace BPM.Web.API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryRepository inventoryRepository, ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        public async Task<InventoryResponseDto> CreateAsync(InventoryCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating inventory for DrugId: {DrugId}, WarehouseId: {WarehouseId}", dto.DrugId, dto.WarehouseId);

                if (dto.ReservedQuantity > dto.Quantity)
                {
                    _logger.LogWarning("Reserved quantity cannot be greater than quantity.");
                    throw new InvalidOperationException("Reserved quantity cannot be greater than quantity.");
                }

                var existingInventory = await _inventoryRepository.GetInventoryForAvailabilityAsync(dto.DrugId, dto.PackagingId, dto.BatchId, dto.WarehouseId);

                if (existingInventory != null)
                {
                    _logger.LogWarning("Inventory already exists for DrugId: {DrugId}, PackagingId: {PackagingId}, BatchId: {BatchId}, WarehouseId: {WarehouseId}", dto.DrugId, dto.PackagingId, dto.BatchId, dto.WarehouseId);
                    throw new InvalidOperationException("Inventory already exists for the specified Drug, Packaging, Batch, and Warehouse.");
                }

                var inventory = InventoryMapper.ToEntity(dto);

                var createdInventory = await _inventoryRepository.CreateAsync(inventory);

                _logger.LogInformation("Inventory created successfully with Id: {InventoryId}", createdInventory.Id);

                return InventoryMapper.ToDto(createdInventory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating inventory.");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all inventories.");

                var inventories = await _inventoryRepository.GetAllAsync();

                return inventories.Where(i => i.IsActive).Select(InventoryMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all inventories.");
                throw;
            }
        }

        public async Task<InventoryResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting inventory with Id: {InventoryId}", id);

                var inventory = await _inventoryRepository.GetByIdAsync(id);

                if (inventory == null || !inventory.IsActive)
                {
                    _logger.LogWarning("Inventory not found with Id: {InventoryId}", id);
                    return null;
                }

                return InventoryMapper.ToDto(inventory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventory with Id: {InventoryId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetByDistributorIdAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Getting inventories for DistributorId: {DistributorId}", distributorId);

                var inventories = await _inventoryRepository.GetByDistributorIdAsync(distributorId);

                return inventories.Where(i => i.IsActive).Select(InventoryMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventories for DistributorId: {DistributorId}", distributorId);
                throw;
            }
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Getting inventories for DrugId: {DrugId}", drugId);

                var inventories = await _inventoryRepository.GetByDrugIdAsync(drugId);

                return inventories.Where(i => i.IsActive).Select(InventoryMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventories for DrugId: {DrugId}", drugId);
                throw;
            }
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetByWarehouseIdAsync(Guid warehouseId)
        {
            try
            {
                _logger.LogInformation("Getting inventories for WarehouseId: {WarehouseId}", warehouseId);

                var inventories = await _inventoryRepository.GetByWarehouseIdAsync(warehouseId);

                return inventories.Where(i => i.IsActive).Select(InventoryMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventories for WarehouseId: {WarehouseId}", warehouseId);
                throw;
            }
        }

        public async Task<InventoryAvailabilityDto> CheckAvailabilityAsync(InventoryAvailabilityDto dto)
        {
            try
            {
                _logger.LogInformation("Checking inventory availability for DrugId: {DrugId}, WarehouseId: {WarehouseId}", dto.DrugId, dto.WarehouseId);

                var inventory = await _inventoryRepository.GetInventoryForAvailabilityAsync(dto.DrugId, dto.PackagingId, dto.BatchId, dto.WarehouseId);

                if (inventory == null)
                {
                    _logger.LogWarning("Inventory not found for DrugId: {DrugId}, PackagingId: {PackagingId}, BatchId: {BatchId}, WarehouseId: {WarehouseId}", dto.DrugId, dto.PackagingId, dto.BatchId, dto.WarehouseId);
                    throw new KeyNotFoundException("Inventory not found.");
                }

                dto.AvailableQuantity = inventory.AvailableQuantity;
                dto.IsAvailable = inventory.AvailableQuantity >= dto.RequestedQuantity;

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking inventory availability.");
                throw;
            }
        }

        public async Task<InventoryResponseDto?> UpdateAsync(InventoryUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating inventory with Id: {InventoryId}", dto.Id);

                var inventory = await _inventoryRepository.GetByIdAsync(dto.Id);

                if (inventory == null || !inventory.IsActive)
                {
                    _logger.LogWarning("Inventory not found with Id: {InventoryId}", dto.Id);
                    return null;
                }

                if (dto.ReservedQuantity > dto.Quantity)
                {
                    _logger.LogWarning("Reserved quantity cannot be greater than quantity.");
                    throw new InvalidOperationException("Reserved quantity cannot be greater than quantity.");
                }

                var updatedInventory = InventoryMapper.ToEntity(dto, inventory);

                await _inventoryRepository.UpdateAsync(updatedInventory);

                _logger.LogInformation("Inventory updated successfully with Id: {InventoryId}", dto.Id);

                return InventoryMapper.ToDto(updatedInventory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory with Id: {InventoryId}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting inventory with Id: {InventoryId}", id);

                var result = await _inventoryRepository.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Inventory not found with Id: {InventoryId}", id);
                    return false;
                }

                _logger.LogInformation("Inventory deleted successfully with Id: {InventoryId}", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory with Id: {InventoryId}", id);
                throw;
            }
        }
    }
}