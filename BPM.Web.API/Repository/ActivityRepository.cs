using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activities>> GetAllAsync()
        {
            return await _context.Activities
                .OrderBy(x => x.ActivityName)
                .ToListAsync();
        }

        public async Task<Activities?> GetByIdAsync(Guid activityId)
        {
            return await _context.Activities
                .FirstOrDefaultAsync(x => x.ActivityId == activityId);
        }

        public async Task<Activities> AddAsync(Activities activity)
        {
            _context.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return activity;
        }

        public async Task<Activities?> UpdateAsync(Activities activity)
        {
            var dbActivity = await _context.Activities
                .FirstOrDefaultAsync(x => x.ActivityId == activity.ActivityId);

            if (dbActivity == null)
            {
                return null;
            }

            dbActivity.ActivityName = activity.ActivityName;
            dbActivity.Code = activity.Code;
            dbActivity.Description = activity.Description;
            dbActivity.IsActive = activity.IsActive;
            dbActivity.ModifiedBy = activity.ModifiedBy;
            dbActivity.ModifiedOn = activity.ModifiedOn;

            await _context.SaveChangesAsync();

            return dbActivity;
        }

        public async Task<bool> DeleteAsync(Guid activityId)
        {
            var dbActivity = await _context.Activities
                .FirstOrDefaultAsync(x => x.ActivityId == activityId);

            if (dbActivity == null)
            {
                return false;
            }

            _context.Activities.Remove(dbActivity);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}