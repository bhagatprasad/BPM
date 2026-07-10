using BPM.Web.API.Models.Data;
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
           var user =  await _context.Users.FindAsync(userId);

            if (user != null)
            {
                user.IsActive = isActive;
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
    }
}
