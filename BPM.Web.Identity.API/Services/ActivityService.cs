using BPM.Web.Identity.API.Models.DTOs;
using BPM.Web.Identity.API.Models.Entities;
using BPM.Web.Identity.API.Repository;

namespace BPM.Web.Identity.API.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ILogger<ActivityService> _logger;

        public ActivityService(
            IActivityRepository activityRepository,
            ILogger<ActivityService> logger)
        {
            _activityRepository = activityRepository;
            _logger = logger;
        }

        public async Task<List<ActivityDto>> GetAllAsync()
        {
            try
            {
                var activities = await _activityRepository.GetAllAsync();

                return activities.Select(a => new ActivityDto
                {
                    ActivityId = a.ActivityId,
                    ActivityName = a.ActivityName,
                    Code = a.Code,
                    Description = a.Description,
                    IsActive = a.IsActive,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedOn = a.ModifiedOn
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting activities");
                throw;
            }
        }

        public async Task<ActivityDto?> GetByIdAsync(Guid activityId)
        {
            try
            {
                var activity = await _activityRepository.GetByIdAsync(activityId);

                if (activity == null)
                    return null;

                return new ActivityDto
                {
                    ActivityId = activity.ActivityId,
                    ActivityName = activity.ActivityName,
                    Code = activity.Code,
                    Description = activity.Description,
                    IsActive = activity.IsActive,
                    CreatedBy = activity.CreatedBy,
                    CreatedOn = activity.CreatedOn,
                    ModifiedBy = activity.ModifiedBy,
                    ModifiedOn = activity.ModifiedOn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting activity");
                throw;
            }
        }

        public async Task<ActivityDto> AddAsync(ActivityCreateDto dto)
        {
            try
            {
                var activity = new Activities
                {
                    ActivityId = Guid.NewGuid(),
                    ActivityName = dto.ActivityName,
                    Code = dto.Code,
                    Description = dto.Description,
                    IsActive = true,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                var result = await _activityRepository.AddAsync(activity);

                return new ActivityDto
                {
                    ActivityId = result.ActivityId,
                    ActivityName = result.ActivityName,
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
                _logger.LogError(ex, "Error while creating activity");
                throw;
            }
        }

        public async Task<ActivityDto?> UpdateAsync(Guid activityId, ActivityUpdateDto dto)
        {
            try
            {
                var activity = new Activities
                {
                    ActivityId = activityId,
                    ActivityName = dto.ActivityName,
                    Code = dto.Code,
                    Description = dto.Description,
                    ModifiedBy = dto.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };

                var result = await _activityRepository.UpdateAsync(activity);

                if (result == null)
                    return null;

                return new ActivityDto
                {
                    ActivityId = result.ActivityId,
                    ActivityName = result.ActivityName,
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
                _logger.LogError(ex, "Error while updating activity");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid activityId)
        {
            try
            {
                return await _activityRepository.DeleteAsync(activityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting activity");
                throw;
            }
        }
    }
}