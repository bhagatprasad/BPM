using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(RefreshToken token)
        {

            var activeTokens = await _context.RefreshTokens.Where(x => x.UserId == token.UserId && !x.IsRevoked).ToListAsync();

            foreach (var t in activeTokens)
            {
                t.IsRevoked = true;
                t.RevokedOn = DateTime.UtcNow;
            }

            await _context.RefreshTokens.AddAsync(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.RefreshTokenValue == refreshToken &&
                    !x.IsRevoked);
        }

        public async Task<bool> UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RevokeAllAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId &&
                            !x.IsRevoked)
                .ToListAsync();

            foreach (var item in tokens)
            {
                item.IsRevoked = true;
                item.RevokedOn = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}