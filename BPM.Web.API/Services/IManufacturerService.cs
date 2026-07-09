using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IManufacturerService
    {
        Task<List<ManufacturerDto>> GetAllManufacturersAsync();

        Task<ManufacturerDto?> GetManufacturerByIdAsync(Guid manufacturerId);

        Task<bool> InsertManufacturerAsync(CreateManufacturerDto manufacturerDto);

        Task<bool> UpdateManufacturerAsync(UpdateManufacturerDto manufacturerDto);

        Task<bool> DeleteManufacturerAsync(Guid manufacturerId);
    }
}
