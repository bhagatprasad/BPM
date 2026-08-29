using BPM.Web.InventoryManagement.API.Models.DTOs;

namespace BPM.Web.InventoryManagement.API.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseResponseDto> CreateAsync(WarehouseCreateDto dto);
        Task<IEnumerable<WarehouseResponseDto>> GetAllAsync();
        Task<WarehouseResponseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<WarehouseResponseDto>> GetByDistributorIdAsync(Guid distributorId);
        Task<WarehouseResponseDto?> UpdateAsync(WarehouseUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
