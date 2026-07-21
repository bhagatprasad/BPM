using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> AuthenticateAsync(string username)
        {
            var user = await _context.Users.Where(x => x.Email.ToLower().Trim() == username.ToLower().Trim() || x.Phone.Trim() == username.Trim()).FirstOrDefaultAsync();

            return user;
        }

        public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.RefreshTokenValue == refreshToken);
        }

        public Task<User> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Attach(user);
            _context.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            _context.Entry(user).Property(x => x.PasswordSalt).IsModified = true;

            await _context.SaveChangesAsync();
        }
    }
}
