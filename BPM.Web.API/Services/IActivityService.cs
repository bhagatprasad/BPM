using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IActivityService
    {
        Task<List<ActivityDto>> GetAllAsync();

        Task<ActivityDto?> GetByIdAsync(Guid activityId);

        Task<ActivityDto> AddAsync(ActivityCreateDto dto);

        Task<ActivityDto?> UpdateAsync(Guid activityId, ActivityUpdateDto dto);

        Task<bool> DeleteAsync(Guid activityId);
    }
}
