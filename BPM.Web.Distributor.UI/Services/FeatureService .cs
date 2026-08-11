using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public FeatureService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<FeatureDto>> GetAllFeaturesAsync()
        {
            return await _repositoryFactory.SendAsync<List<FeatureDto>>(
                HttpMethod.Get,
                "Feature");
        }

        public async Task<FeatureDto?> GetFeatureByIdAsync(Guid featureId)
        {
            return await _repositoryFactory.SendAsync<FeatureDto>(
                HttpMethod.Get,
                $"Feature/{featureId}");
        }

        public async Task<FeatureDto> CreateFeatureAsync(FeatureCreateDto dto)
        {
            return await _repositoryFactory.SendAsync<FeatureCreateDto, FeatureDto>(
                HttpMethod.Post,
                "Feature",
                dto);
        }

        public async Task<FeatureDto?> UpdateFeatureAsync(Guid featureId, FeatureUpdateDto dto)
        {
            return await _repositoryFactory.SendAsync<FeatureUpdateDto, FeatureDto>(
                HttpMethod.Put,
                $"Feature/{featureId}",
                dto);
        }

    }
}