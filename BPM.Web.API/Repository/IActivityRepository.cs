using BPM.Web.API.Models.Entities;
using System.Diagnostics;

namespace BPM.Web.API.Repository
{
    public interface IActivityRepository
    {
        Task<List<Activities>> GetAllAsync();

        Task<Activities?> GetByIdAsync(Guid activityId);

        Task<Activities> AddAsync(Activities activity);

        Task<Activities?> UpdateAsync(Activities activity);

        Task<bool> DeleteAsync(Guid activityId);
    }
}
