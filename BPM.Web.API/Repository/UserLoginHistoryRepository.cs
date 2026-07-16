using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class UserLoginHistoryRepository : IUserLoginHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public UserLoginHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(UserLoginHistory loginHistory)
        {
            loginHistory.CreatedOn = DateTime.UtcNow;

            _context.UserLoginHistorys.Add(loginHistory);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserLoginHistory>> GetAllAsync()
        {
            return await _context.UserLoginHistorys
                .OrderByDescending(x => x.LoginTime)
                .ToListAsync();
        }

        public async Task<UserLoginHistory?> GetByIdAsync(Guid id)
        {
            return await _context.UserLoginHistorys
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UserLoginHistory>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserLoginHistorys
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LoginTime)
                .ToListAsync();
        }
    }
}
