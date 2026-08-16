using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using BPM.Web.API.Services.Interfaces;

namespace BPM.Web.API.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILogger<WarehouseService> _logger;

        public WarehouseService(IWarehouseRepository warehouseRepository, ILogger<WarehouseService> logger)
        {
            _warehouseRepository = warehouseRepository;
            _logger = logger;
        }

        public async Task<WarehouseResponseDto> CreateAsync(WarehouseCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creating warehouse with Code: {WarehouseCode}", dto.WarehouseCode);

                var existingWarehouse = await _warehouseRepository.GetByCodeAsync(dto.WarehouseCode);

                if (existingWarehouse != null)
                {
                    _logger.LogWarning("Warehouse already exists with Code: {WarehouseCode}", dto.WarehouseCode);
                    throw new InvalidOperationException("Warehouse code already exists.");
                }

                var warehouse = WarehouseMapper.ToEntity(dto);

                var createdWarehouse = await _warehouseRepository.CreateAsync(warehouse);

                _logger.LogInformation("Warehouse {WarehouseId} created successfully.", createdWarehouse.Id);

                return WarehouseMapper.ToDto(createdWarehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating warehouse with Code: {WarehouseCode}", dto.WarehouseCode);
                throw;
            }
        }

        public async Task<IEnumerable<WarehouseResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all warehouses.");

                var warehouses = await _warehouseRepository.GetAllAsync();

                _logger.LogInformation("Successfully retrieved {Count} warehouses.", warehouses.Count);

                return WarehouseMapper.ToDtoList(warehouses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all warehouses.");
                throw;
            }
        }

        public async Task<WarehouseResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching warehouse with Id: {WarehouseId}", id);

                var warehouse = await _warehouseRepository.GetByIdAsync(id);

                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with Id: {WarehouseId}", id);
                    return null;
                }

                _logger.LogInformation("Warehouse found with Id: {WarehouseId}", id);

                return WarehouseMapper.ToDto(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching warehouse with Id: {WarehouseId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<WarehouseResponseDto>> GetByDistributorIdAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Getting warehouses for DistributorId: {DistributorId}", distributorId);

                var warehouses = await _warehouseRepository.GetByDistributorIdAsync(distributorId);

                if (warehouses.Count == 0)
                {
                    _logger.LogWarning("No warehouses found for DistributorId: {DistributorId}", distributorId);
                }

                return WarehouseMapper.ToDtoList(warehouses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting warehouses for DistributorId: {DistributorId}", distributorId);
                throw;
            }
        }

        public async Task<WarehouseResponseDto?> UpdateAsync(WarehouseUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating warehouse with Id: {WarehouseId}", dto.Id);

                var warehouse = await _warehouseRepository.GetByIdAsync(dto.Id);

                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with Id: {WarehouseId}", dto.Id);
                    return null;
                }

                WarehouseMapper.ToEntity(dto, warehouse);

                await _warehouseRepository.UpdateAsync(warehouse);

                _logger.LogInformation("Warehouse {WarehouseId} updated successfully.", dto.Id);

                return WarehouseMapper.ToDto(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating warehouse with Id: {WarehouseId}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting warehouse with Id: {WarehouseId}", id);

                var warehouse = await _warehouseRepository.GetByIdAsync(id);

                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with Id: {WarehouseId}", id);
                    return false;
                }

                var result = await _warehouseRepository.DeleteAsync(id);

                if (result)
                {
                    _logger.LogInformation("Warehouse {WarehouseId} deleted successfully.", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting warehouse with Id: {WarehouseId}", id);
                throw;
            }
        }
    }
}
