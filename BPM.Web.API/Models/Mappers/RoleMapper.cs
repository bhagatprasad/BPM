using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto ToDto(this Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Code = role.Code,
                IsActive = role.IsActive
            };
        }

        public static List<RoleDto> ToDto(this IEnumerable<Role> roles)
        {
            return roles.Select(r => r.ToDto()).ToList();
        }

        public static Role ToEntity(this CreateRoleDto dto)
        {
            return new Role
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Code = dto.Code,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Role ToEntity(this UpdateRoleDto dto)
        {
            return new Role
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code,
                IsActive = dto.IsActive,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public static void MapToEntity(this UpdateRoleDto dto, Role role)
        {
            role.Name = dto.Name;
            role.Code = dto.Code;
            role.IsActive = dto.IsActive;
            role.ModifiedOn = DateTime.UtcNow;
        }
    }
}
