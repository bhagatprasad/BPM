using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IDistributorRepository
    {
        Task<Distributor> InsertDistributorAsync(Distributor distributor);
        Task<Distributor?> GetDistributorByIdAsync(Guid distributorId);
        Task<List<Distributor>> GetAllDistributorsAsync();
        Task<bool> UpdateDistributorAsync(Distributor distributor);
        Task<bool> DeleteDistributorAsync(Guid distributorId);

    }
}
