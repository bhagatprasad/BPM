using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Repository
{
    public interface IAuthRepository
    {
        Task<AuthResponse> AuthenticateUser(string username, string password);
        Task<AuthenticateResponseDto> GenarateUserClaims(AuthResponse authResponse);
    }
}
