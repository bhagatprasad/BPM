using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Services
{
    public interface IDistributorService
    {
        Task<DistributorDto> InsertDistributorAsync(CreateDistributorDto distributorDto);
        Task<DistributorDto> GetDistributorByIdAsync(Guid distributorId);
        Task<List<DistributorDto>> GetDistributorListAsync();
        Task<DistributorDto> UpdateDistributorAsync(Guid distributorId, UpdateDistributorDto updateDistributor);
        Task<bool> DeleteDistributorById(Guid distributorId);
    }
}
