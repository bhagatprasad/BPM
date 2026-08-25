using BPM.Web.Identity.API.Models.DTOs;

namespace BPM.Web.Identity.API.Services
{
    public interface IFeatureService
    {
        Task<List<FeatureDto>> GetAllAsync();

        Task<FeatureDto?> GetByIdAsync(Guid featureId);

        Task<FeatureDto> AddAsync(FeatureCreateDto dto);

        Task<FeatureDto?> UpdateAsync(Guid featureId, FeatureUpdateDto dto);

        Task<bool> DeleteAsync(Guid featureId);
    }
}
