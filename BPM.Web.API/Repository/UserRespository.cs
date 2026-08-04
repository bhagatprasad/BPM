using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class UserRespository : IUserRespository
    {
        private readonly ApplicationDbContext _context;
        public UserRespository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetUserListByDealerAsync(Guid dealerId)
        {
            return await _context.Users.Where(x => x.DealerId == dealerId).ToListAsync();
        }

        public async Task<bool> ActivateUserAync(User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.IsActive = user.IsActive;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserInfoAsync(User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeactivateUserAync(User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.IsActive = user.IsActive;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserRoleAsync(User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.RoleId = user.RoleId;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserDealerAsync(User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.DealerId = user.DealerId;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePasswordAsync(User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                return false;
            }
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.PasswordSalt = user.PasswordSalt;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByUsernameOrPhoneAsync(string username, string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == username || x.Phone == phone);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(x=>x.Role).Include(x=>x.Dealer).ToListAsync();
        }
    }
}
