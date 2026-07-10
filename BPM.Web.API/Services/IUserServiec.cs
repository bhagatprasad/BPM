
using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IUserServiec
    {
        Task<bool> InsertUserAsync(UserCreateDto user);
        Task<bool> ActivateUserAync(UserActivateDto userActivateDto);
    }
}
