using BPM.Web.API.Models.DTOs;

namespace BPM.Web.API.Services
{
    public interface IAccountService
    {
        Task<AuthResponse> AuthenticateAsync(AuthenticateUserDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task<ForgotPasswordResponseDto> IdentifyUserAsync(ForgotPasswordDto dto);

        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }
}
