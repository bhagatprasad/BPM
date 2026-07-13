using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IAccountService
    {
        Task<AuthResponse> AuthenticateAsync(AuthenticateUserDto dto);
    }
}
