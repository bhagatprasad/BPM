using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public interface IAuthenticateService
    {
        Task<AuthResponse> AuthenticateUserAsync(AuthenticateUserDto dto);

        Task<ApplicationUser> GenerateUserClaimsAsync(AuthResponse auth);
        Task<ForgotPasswordResponseDto?> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);

        Task<RefreshTokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto);
    }
}