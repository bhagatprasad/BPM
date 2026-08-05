using BPM.Web.API.Models.Data;
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
    }
}
