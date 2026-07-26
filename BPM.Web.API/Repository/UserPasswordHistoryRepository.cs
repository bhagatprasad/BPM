using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class UserPasswordHistoryRepository : IUserPasswordHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPasswordHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserPasswordHistory>> GetLastFivePasswordsAsync(Guid userId)
        {
            return await _context.UserPasswordHistories.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedOn).Take(5).ToListAsync();
        }

        public async Task AddAsync(UserPasswordHistory history)
        {
            await _context.UserPasswordHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOldPasswordsAsync(Guid userId)
        {
            var oldPasswords = await _context.UserPasswordHistories.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedOn).Skip(5).ToListAsync();

            if (oldPasswords.Any())
            {
                _context.UserPasswordHistories.RemoveRange(oldPasswords);
                await _context.SaveChangesAsync();
            }
        }
    }
}
