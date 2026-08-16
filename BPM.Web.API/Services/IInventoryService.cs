using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryResponseDto> CreateAsync(InventoryCreateDto dto);
        Task<IEnumerable<InventoryResponseDto>> GetAllAsync();
        Task<InventoryResponseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<InventoryResponseDto>> GetByDistributorIdAsync(Guid distributorId);
        Task<IEnumerable<InventoryResponseDto>> GetByDrugIdAsync(Guid drugId);
        Task<IEnumerable<InventoryResponseDto>> GetByWarehouseIdAsync(Guid warehouseId);
        Task<InventoryAvailabilityDto> CheckAvailabilityAsync(InventoryAvailabilityDto dto);
        Task<InventoryResponseDto?> UpdateAsync(InventoryUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
