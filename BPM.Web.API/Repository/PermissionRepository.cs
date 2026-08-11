using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(Guid permissionId)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(x => x.PermissionId == permissionId);
        }

        public async Task<Permission> AddAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<Permission?> UpdateAsync(Permission permission)
        {
            var dbPermission = await _context.Permissions
                .FindAsync(permission.PermissionId);

            if (dbPermission == null)
                return null;

            dbPermission.RoleId = permission.RoleId;
            dbPermission.FeatureId = permission.FeatureId;
            dbPermission.ActivityId = permission.ActivityId;
            dbPermission.IsEnabled = permission.IsEnabled;
            dbPermission.ModifiedBy = permission.ModifiedBy;
            dbPermission.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return dbPermission;
        }

        public async Task<bool> DeleteAsync(Guid permissionId)
        {
            var dbPermission = await _context.Permissions
                .FindAsync(permissionId);

            if (dbPermission == null)
                return false;

            _context.Permissions.Remove(dbPermission);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasPermissionAsync(Guid roleId, string featureCode, string activityCode)
        {
            return await (
                from p in _context.Permissions
                join f in _context.Features
                    on p.FeatureId equals f.FeatureId
                join a in _context.Activities
                    on p.ActivityId equals a.ActivityId
                where p.RoleId == roleId
                      && p.IsEnabled
                      && f.Code == featureCode
                      && a.Code == activityCode
                select p
            ).AnyAsync();
        }

        public async Task<List<PermissionFeatureDto>> GetPermissionsByRoleAsync(Guid roleId)
        {
            var permissions = await _context.Permissions
                .Include(x => x.Feature)
                .Include(x => x.Activity)
                .Where(x => x.RoleId == roleId)
                .OrderBy(x => x.Feature.FeatureName)
                .ThenBy(x => x.Activity.ActivityName)
                .ToListAsync();

            var result = permissions
                .GroupBy(x => new
                {
                    x.FeatureId,
                    x.Feature.FeatureName,
                    x.Feature.Code
                })
                .Select(g => new PermissionFeatureDto
                {
                    FeatureId = g.Key.FeatureId,
                    FeatureName = g.Key.FeatureName,
                    FeatureCode = g.Key.Code,

                    Activities = g.Select(a => new PermissionActivityDto
                    {
                        PermissionId = a.PermissionId,
                        ActivityId = a.ActivityId,
                        ActivityName = a.Activity.ActivityName,
                        ActivityCode = a.Activity.Code,
                        IsEnabled = a.IsEnabled
                    }).ToList()
                })
                .ToList();

            return result;
        }
    }
}
