using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<List<ManufacturerDto>> GetAllManufacturersAsync()
        {
            var manufacturers = await _manufacturerRepository.GetAllManufacturersAsync();

            return manufacturers.ToDto();
        }

        public async Task<ManufacturerDto?> GetManufacturerByIdAsync(Guid manufacturerId)
        {
            var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId);

            if (manufacturer == null)
                return null;

            return manufacturer.ToDto();
        }

        public async Task<bool> InsertManufacturerAsync(CreateManufacturerDto manufacturerDto)
        {
            var manufacturer = manufacturerDto.ToEntity();

            return await _manufacturerRepository.InsertManufacturerAsync(manufacturer);
        }

        public async Task<bool> UpdateManufacturerAsync(UpdateManufacturerDto manufacturerDto)
        {
            var manufacturer = manufacturerDto.ToEntity();

            return await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);
        }

        public async Task<bool> DeleteManufacturerAsync(Guid manufacturerId)
        {
            return await _manufacturerRepository.DeleteManufacturerAsync(manufacturerId);
        }
    }
}