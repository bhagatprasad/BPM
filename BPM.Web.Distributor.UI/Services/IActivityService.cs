using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IActivityService
    {
        Task<List<ActivityDto>> GetAllActivitiesAsync();

        Task<ActivityDto?> GetActivityByIdAsync(Guid activityId);

        Task<ActivityDto?> CreateActivityAsync(ActivityCreateDto dto);

        Task<ActivityDto?> UpdateActivityAsync(Guid activityId, ActivityUpdateDto dto);
           
    }
}