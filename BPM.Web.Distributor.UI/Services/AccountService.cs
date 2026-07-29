using BPM.Web.Distributor.UI.Helpers;
using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BPM.Web.Distributor.UI.Services
{
    public class AccountService
    {
        private readonly HttpClient _client;

        public AccountService(HttpClientService httpClientService)
        {
            _client = httpClientService.GetHttpClient();
        }

        public async Task<AuthResponse?> LoginAsync(AuthenticateUserDto dto)
        {
            var response = await _client.PostAsJsonAsync(
                "Account/authenticate",
                dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<ForgotPasswordResponseDto?> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var response = await _client.PostAsJsonAsync(
                "Account/forgot-password",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                return new ForgotPasswordResponseDto
                {
                    Success = false,
                    Message = "Unable to connect to server."
                };
            }

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
    }
}