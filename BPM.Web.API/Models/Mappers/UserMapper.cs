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
                DistributorId = dto.DistributorId,
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
        public static User ToEntity(this UserUpdateDto dto)
        {
            return new User
            {
                Id = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                IsActive = dto.IsActive ?? false,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public static List<UserDto> ToUserDtoList(this List<User> users)
        {
            return users.Select(u => u.ToEntity()).ToList();
        }

        public static UserDto ToEntity(this User dto)
        {
            return new UserDto
            {
                UserId = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                IsActive = dto.IsActive,
                RoleId = dto.RoleId,
                DealerId = dto.DealerId,
                DistributorId = dto.DistributorId,
                DealerInfo = dto.Dealer != null ? dto.Dealer.ToDto() : null,
                DistributorInfo = dto.Distributor != null ? dto.Distributor.ToDto() : null,               
                RoleInfo = dto.Role != null ? dto.Role.ToDto() : null,
            };

        }

        public static User ToEntity(this UserActivateDto dto)
        {
            return new User
            {
                Id = dto.UserId,
                IsActive = true,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public static User ToEntity(this UserDeactivateDto dto)
        {
            return new User
            {
                Id = dto.UserId,
                IsActive = false,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };
        }
        public static User ToEntity(this UserRoleUpdateDto dto)
        {
            return new User
            {
                Id = dto.UserId,
                RoleId = dto.RoleId,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public static User ToEntity(this UserDealerUpdateDto dto)
        {
            return new User
            {
                Id = dto.UserId,
                DealerId = dto.DealerId,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };
        }



    }
}
