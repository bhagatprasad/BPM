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
    }
}
