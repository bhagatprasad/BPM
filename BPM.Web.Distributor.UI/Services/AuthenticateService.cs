using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public AuthenticateService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<AuthResponse?> AuthenticateUserAsync(AuthenticateUserDto dto)
        {
            return await _repositoryFactory.SendAsync<AuthenticateUserDto, AuthResponse>(HttpMethod.Post, "Account/authenticate", dto);
        }

        public async Task<ForgotPasswordResponseDto?> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            return await _repositoryFactory.SendAsync<ForgotPasswordDto, ForgotPasswordResponseDto>(
                HttpMethod.Post,
                "Account/forgot-password",
                dto);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            return await _repositoryFactory.SendAsync<ResetPasswordDto, bool>(
                HttpMethod.Post,
                "Account/reset-password",
                dto);
        }

        public async Task<RefreshTokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            return await _repositoryFactory.SendAsync<RefreshTokenRequestDto, RefreshTokenResponseDto>(
                HttpMethod.Post,
                "Account/refresh-token",
                dto);
        }

        public async Task<ApplicationUser> GenerateUserClaimsAsync(AuthResponse auth)
        {
            return await _repositoryFactory.SendAsync<AuthResponse, ApplicationUser>(
                HttpMethod.Post,
                "Account/GenerateUserClaimsAsync",
                auth);
        }
    }
}