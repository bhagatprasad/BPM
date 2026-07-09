using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IManufacturerRepository
    {
        Task<List<Manufacturer>> GetAllManufacturersAsync();

        Task<Manufacturer?> GetManufacturerByIdAsync(Guid manufacturerId);

        Task<bool> InsertManufacturerAsync(Manufacturer manufacturer);

        Task<bool> UpdateManufacturerAsync(Manufacturer manufacturer);

        Task<bool> DeleteManufacturerAsync(Guid manufacturerId);
    }
}
