using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace BPM.Web.Distributor.UI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly HttpClient _client;

        public AuthenticateService(HttpClient client)
        {
            _client = client;
        }

        public async Task<AuthResponse?> AuthenticateUserAsync(AuthenticateUserDto dto)
        {
            var response = await _client.PostAsJsonAsync(
                "Account/authenticate",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthResponse
                {
                    IsValidUser = false,
                    IsValidPassword = false,
                    Message = "Unable to connect to server."
                };
            }
            return await response.Content.ReadFromJsonAsync<AuthResponse>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        public async Task<ForgotPasswordResponseDto?> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var response = await _client.PostAsJsonAsync(
                "Account/forgot-password",
                dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ForgotPasswordResponseDto>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var response = await _client.PostAsJsonAsync(
                "Account/reset-password",
                dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<RefreshTokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var response = await _client.PostAsJsonAsync(
                "Account/refresh-token",
                dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<RefreshTokenResponseDto>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        public Task<ApplicationUser> GenerateUserClaimsAsync(AuthResponse auth)
        {
            var dto = auth.AuthenticateResponseDto;

            var user = new ApplicationUser
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                DealerId = dto.DealerId,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive,
                JwtToken = auth.JwtToken,
                RefreshToken = auth.RefreshToken
            };

            return Task.FromResult(user);
        }
    }
}