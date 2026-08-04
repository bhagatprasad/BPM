using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly ILogger<FeatureService> _logger;

        public FeatureService(
            IFeatureRepository featureRepository,
            ILogger<FeatureService> logger)
        {
            _featureRepository = featureRepository;
            _logger = logger;
        }

        public async Task<List<FeatureDto>> GetAllAsync()
        {
            try
            {
                var features = await _featureRepository.GetAllAsync();

                return features.Select(x => new FeatureDto
                {
                    FeatureId = x.FeatureId,
                    FeatureName = x.FeatureName,
                    Code = x.Code,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting features.");
                throw;
            }
        }

        public async Task<FeatureDto?> GetByIdAsync(Guid featureId)
        {
            try
            {
                var feature = await _featureRepository.GetByIdAsync(featureId);

                if (feature == null)
                    return null;

                return new FeatureDto
                {
                    FeatureId = feature.FeatureId,
                    FeatureName = feature.FeatureName,
                    Code = feature.Code,
                    Description = feature.Description,
                    IsActive = feature.IsActive,
                    CreatedBy = feature.CreatedBy,
                    CreatedOn = feature.CreatedOn,
                    ModifiedBy = feature.ModifiedBy,
                    ModifiedOn = feature.ModifiedOn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting feature.");
                throw;
            }
        }

        public async Task<FeatureDto> AddAsync(FeatureCreateDto dto)
        {
            try
            {
                var feature = new Feature
                {
                    FeatureId = Guid.NewGuid(),
                    FeatureName = dto.FeatureName,
                    Code = dto.Code,
                    Description = dto.Description,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _featureRepository.AddAsync(feature);

                return new FeatureDto
                {
                    FeatureId = result.FeatureId,
                    FeatureName = result.FeatureName,
                    Code = result.Code,
                    Description = result.Description,
                    IsActive = result.IsActive,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedOn = result.ModifiedOn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating feature.");
                throw;
            }
        }

        public async Task<FeatureDto?> UpdateAsync(Guid featureId, FeatureUpdateDto dto)
        {
            try
            {
                var feature = new Feature
                {
                    FeatureId = featureId,
                    FeatureName = dto.FeatureName,
                    Code = dto.Code,
                    Description = dto.Description,
                    ModifiedBy = dto.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };

                var result = await _featureRepository.UpdateAsync(feature);

                if (result == null)
                    return null;

                return new FeatureDto
                {
                    FeatureId = result.FeatureId,
                    FeatureName = result.FeatureName,
                    Code = result.Code,
                    Description = result.Description,
                    IsActive = result.IsActive,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedOn = result.ModifiedOn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating feature.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid featureId)
        {
            try
            {
                return await _featureRepository.DeleteAsync(featureId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting feature.");
                throw;
            }
        }
    }
}