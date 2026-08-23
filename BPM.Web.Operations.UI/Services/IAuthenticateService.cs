using BPM.Web.Operations.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Services
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
