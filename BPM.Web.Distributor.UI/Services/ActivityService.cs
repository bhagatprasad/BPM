using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public ActivityService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<ActivityDto>> GetAllActivitiesAsync()
        {
            return await _repositoryFactory.SendAsync<List<ActivityDto>>(
                HttpMethod.Get,
                "Activity");
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid activityId)
        {
            return await _repositoryFactory.SendAsync<ActivityDto>(
                HttpMethod.Get,
                $"Activity/{activityId}");
        }

        public async Task<ActivityDto?> CreateActivityAsync(ActivityCreateDto dto)
        {
            return await _repositoryFactory.SendAsync<ActivityCreateDto, ActivityDto>(
                HttpMethod.Post,
                "Activity",
                dto);
        }

        public async Task<ActivityDto?> UpdateActivityAsync(Guid activityId, ActivityUpdateDto dto)
        {
            return await _repositoryFactory.SendAsync<ActivityUpdateDto, ActivityDto>(
                HttpMethod.Put,
                $"Activity/{activityId}",
                dto);
        }

    }
}