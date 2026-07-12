using BPM.Web.API.Helpes;
using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;

namespace BPM.Web.API.Repository
{
    public class UserRespository : IUserRespository
    {
        private readonly ApplicationDbContext _context;
        public UserRespository(ApplicationDbContext context)
        {
            _context = context;
        }

      public async Task<bool> ActivateUserAync(UserActivateDto userActivateDto)
        {
            var user = await _context.Users.FindAsync(userActivateDto.UserId);

            if (user != null)
            {
                user.UserActivateDto(userActivateDto);
            }

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateUserInfoAsync(UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(userUpdateDto.UserId);
            if (user != null)
            {
                user.UpdateUserInfo(userUpdateDto);
            }
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto)
        {
            var user = await _context.Users.FindAsync(userDeactivateDto.UserId);
            if (user != null)
            {
                user.UserDeactivateDto(userDeactivateDto);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

       

        public async Task<bool> UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto)
        {
            var user = await _context.Users.FindAsync(userRoleUpdateDto.UserId);
            if (user != null)
            {
                user.UserRoleUpdateDto(userRoleUpdateDto);
            }
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto)
        {
            var user = await _context.Users.FindAsync(userDealerUpdateDto.UserId);

            if (user != null)
            {
                user.UserDealerUpdateDto(userDealerUpdateDto);
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

            dbUser.UpdatePassword(user);
            
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
