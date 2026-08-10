using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IFeatureService
    {
        Task<List<FeatureDto>> GetAllFeaturesAsync();

        Task<FeatureDto?> GetFeatureByIdAsync(Guid featureId);

        Task<FeatureDto> CreateFeatureAsync(FeatureCreateDto dto);

        Task<FeatureDto?> UpdateFeatureAsync(Guid featureId, FeatureUpdateDto dto);

    }
}