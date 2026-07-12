using BPM.Web.API.Helpes;
using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public class UserRespository : IUserRespository
    {
        private readonly ApplicationDbContext _context;
        public UserRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ActivateUserAync(Guid userId, bool isActive, Guid modifiedBy)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                user.IsActive = isActive;
                user.ModifiedBy = modifiedBy;
                user.ModifiedOn = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeactivateUserAync(Guid userId, Guid modifiedBy)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                user.ModifiedBy = modifiedBy;
                user.ModifiedOn = DateTime.UtcNow;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserInfoAsync(UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(userUpdateDto.UserId);
            if (user != null)
            {
                user.FirstName = userUpdateDto.FirstName;
                user.LastName = userUpdateDto.LastName;
                user.Email = userUpdateDto.Email;
                user.Phone = userUpdateDto.Phone;
                user.ModifiedBy = userUpdateDto.ModifiedBy;
                user.ModifiedOn = DateTime.UtcNow;
            }
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto)
        {
            var user = await _context.Users.FindAsync(userRoleUpdateDto.UserId);
            if (user != null)
            {
                user.RoleId = userRoleUpdateDto.RoleId;
                user.ModifiedBy = userRoleUpdateDto.ModifiedBy;
                user.ModifiedOn = DateTime.UtcNow;
            }
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto)
        {
            var user = await _context.Users.FindAsync(userDealerUpdateDto.UserId);

            if (user != null)
            {
                user.DealerId = userDealerUpdateDto.DealerId;
                user.ModifiedBy = userDealerUpdateDto.ModifiedBy;
                user.ModifiedOn = DateTime.UtcNow;
            }
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
            dbUser.ModifiedOn = user.ModifiedOn;
            dbUser.ModifiedBy = user.ModifiedBy;
            
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
