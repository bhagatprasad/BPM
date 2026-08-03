using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersListAsync();
        Task<bool> InsertUserAsync(UserCreateDto user);
        Task<bool> ActivateUserAync(UserActivateDto userActivateDto);
        Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto);
        Task<UserDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto);
        Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto);
    }
}
