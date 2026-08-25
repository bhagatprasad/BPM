using BPM.Web.Identity.API.Models.Entities;

namespace BPM.Web.Identity.API.Repository
{
    public interface IFeatureRepository
    {
        Task<List<Feature>> GetAllAsync();

        Task<Feature?> GetByIdAsync(Guid featureId);

        Task<Feature> AddAsync(Feature feature);

        Task<Feature?> UpdateAsync(Feature feature);

        Task<bool> DeleteAsync(Guid featureId);
    }
}
