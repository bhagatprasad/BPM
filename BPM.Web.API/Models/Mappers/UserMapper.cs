using BPM.Web.API.Helpes;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this UserCreateDto dto)
        {
            HashSalt passwordHashSalt = HashSalt.GenerateSaltedHash(dto.Password);

            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                RoleId = dto.RoleId,
                PasswordHash = passwordHashSalt.Hash,
                PasswordSalt = passwordHashSalt.Salt,
                DealerId = dto.DealerId,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static User ToEntity(this UserChangePasswordDto dto)
        {
            HashSalt passwordHashSalt = HashSalt.GenerateSaltedHash(dto.NewPassword);

            return new User
            {
                Id = dto.UserId,
                PasswordHash = passwordHashSalt.Hash,
                PasswordSalt = passwordHashSalt.Salt,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = dto.ModifiedBy
            };
        }
        public static void UpdateUserInfo(this User user, UserUpdateDto dto)
        {
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.ModifiedBy = dto.ModifiedBy;
            user.ModifiedOn = DateTime.UtcNow;
        }
        public static void UserActivateDto(this User user, UserActivateDto dto)
        {
            user.IsActive = true;
            user.ModifiedBy = dto.ModifiedBy;
            user.ModifiedOn = DateTime.UtcNow;
        }
        public static void UserDeactivateDto(this User user, UserDeactivateDto dto)
        {
            user.IsActive = false;
            user.ModifiedBy = dto.ModifiedBy;
            user.ModifiedOn = DateTime.UtcNow;
        }
        public static void UserRoleUpdateDto(this User user, UserRoleUpdateDto dto)
        {
            user.RoleId = dto.RoleId;
            user.ModifiedBy = dto.ModifiedBy;
            user.ModifiedOn = DateTime.UtcNow;
        }
        public static void UserDealerUpdateDto(this User user, UserDealerUpdateDto dto)
        {
            user.DealerId = dto.DealerId;
            user.ModifiedBy = dto.ModifiedBy;
            user.ModifiedOn = DateTime.UtcNow;
        }
        public static void UpdatePassword(this User dbUser,User user) 
        {
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.PasswordSalt = user.PasswordSalt;
            dbUser.ModifiedBy = user.ModifiedBy;
            dbUser.ModifiedOn = user.ModifiedOn;
        }


    }
}
