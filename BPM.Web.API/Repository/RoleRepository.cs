using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == roleId);
        }

        public async Task<bool> InsertRoleAsync(Role role)
        {
            role.CreatedOn = DateTime.UtcNow;
            role.IsActive = true;

            _context.Roles.Add(role);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            var existingRole = await _context.Roles.FindAsync(role.Id);

            if (existingRole == null)
                return false;

            existingRole.Name = role.Name;
            existingRole.IsActive = role.IsActive;
            existingRole.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);

            if (role == null)
                return false;

            _context.Roles.Remove(role);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
