using BPM.Web.Operations.UI.Models;
using System.Net.Http;

namespace BPM.Web.Operations.UI.Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        public DistributorService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public async Task<bool> DeleteDistributorById(Guid distributorId)
        {

            return await _repositoryFactory.SendAsync<bool>(HttpMethod.Delete, $"distributor/delete-distributor/{distributorId}");
        }

        public async Task<DistributorDto> GetDistributorByIdAsync(Guid distributorId)
        {
            return await _repositoryFactory.SendAsync<DistributorDto>(HttpMethod.Get, $"distributor/get-distributor-by-id/{distributorId}");
        }

        public async Task<List<DistributorDto>> GetDistributorListAsync()
        {
            return await _repositoryFactory.SendAsync<List<DistributorDto>>(HttpMethod.Get, "distributor/get-all-distributors");
        }

        public async Task<DistributorDto> InsertDistributorAsync(CreateDistributorDto distributorDto)
        {
            return await _repositoryFactory.SendAsync<CreateDistributorDto,DistributorDto>(HttpMethod.Post, "distributor/insert-distributor");
        }

        public async Task<DistributorDto> UpdateDistributorAsync(Guid distributorId, UpdateDistributorDto updateDistributor)
        {
            return await _repositoryFactory.SendAsync<UpdateDistributorDto, DistributorDto>(HttpMethod.Post, $"distributor/update-distributor/{distributorId}");
        }
    }
}
