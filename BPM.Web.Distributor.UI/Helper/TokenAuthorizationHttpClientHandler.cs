using BPM.Web.Distributor.UI.Models;
using BPM.Web.Distributor.UI.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BPM.Web.Distributor.UI.Helpers
{
    public class TokenAuthorizationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly BPMConfig _config;
        private readonly ILogger<TokenAuthorizationHttpClientHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public TokenAuthorizationHttpClientHandler(
            IHttpContextAccessor contextAccessor,
            IOptions<BPMConfig> config,
            ILogger<TokenAuthorizationHttpClientHandler> logger,
            IHttpClientFactory httpClientFactory)
        {
            _contextAccessor = contextAccessor;
            _config = config.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authResponse = GetAuthResponseFromSession();

            if (authResponse != null && !string.IsNullOrWhiteSpace(authResponse.JwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.JwtToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // If 401 Unauthorized, try to refresh token
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Received 401 Unauthorized. Attempting to refresh token.");

                // Use semaphore to prevent multiple concurrent refresh attempts
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    // Check if the token has been refreshed by another thread
                    var updatedAuthResponse = GetAuthResponseFromSession();
                    if (updatedAuthResponse != null &&
                        !string.IsNullOrWhiteSpace(updatedAuthResponse.JwtToken) &&
                        updatedAuthResponse.JwtToken != authResponse?.JwtToken)
                    {
                        // Token was already refreshed, retry with new token
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", updatedAuthResponse.JwtToken);
                        response = await base.SendAsync(request, cancellationToken);
                        return response;
                    }

                    // Refresh the token
                    var refreshed = await RefreshTokenAsync(cancellationToken);
                    if (refreshed)
                    {
                        var refreshedAuthResponse = GetAuthResponseFromSession();
                        if (refreshedAuthResponse != null && !string.IsNullOrWhiteSpace(refreshedAuthResponse.JwtToken))
                        {
                            // Retry the original request with new token
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedAuthResponse.JwtToken);
                            response = await base.SendAsync(request, cancellationToken);
                            _logger.LogInformation("Token refreshed and request retried successfully.");
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during token refresh.");
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return response;
        }

        private async Task<bool> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            try
            {
                var authResponse = GetAuthResponseFromSession();
                if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.RefreshToken))
                {
                    _logger.LogWarning("No refresh token available in session.");
                    await ClearSessionAndRedirectAsync();
                    return false;
                }

                var refreshToken = authResponse.RefreshToken;

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_config.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);

                // Use the API endpoint from the controller
                var request = new HttpRequestMessage(HttpMethod.Post, "Account/refresh-token");

                var refreshRequest = new RefreshTokenRequestDto
                {
                    RefreshToken = refreshToken
                };

                var content = new StringContent(JsonConvert.SerializeObject(refreshRequest), Encoding.UTF8, "application/json"); request.Content = content;

                var response = await client.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    var newAuthResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

                    if (newAuthResponse != null && !string.IsNullOrWhiteSpace(newAuthResponse.JwtToken))
                    {
                        // Update session with new tokens
                        _contextAccessor.HttpContext?.Session.SetString("JwtToken", newAuthResponse.JwtToken);

                        // Only update refresh token if a new one is provided
                        if (!string.IsNullOrWhiteSpace(newAuthResponse.RefreshToken))
                        {
                            _contextAccessor.HttpContext?.Session.SetString("RefreshToken", newAuthResponse.RefreshToken);
                        }

                        // Update the stored AuthResponse with new tokens
                        var currentAuthResponse = GetAuthResponseFromSession();

                        if (currentAuthResponse != null)
                        {
                            currentAuthResponse.JwtToken = newAuthResponse.JwtToken;
                            if (!string.IsNullOrWhiteSpace(newAuthResponse.RefreshToken))
                            {
                                currentAuthResponse.RefreshToken = newAuthResponse.RefreshToken;
                            }
                            SetAuthResponseInSession(currentAuthResponse);
                        }
                        else
                        {
                            // If somehow AuthResponse is not in session, set it
                            SetAuthResponseInSession(newAuthResponse);
                        }

                        _logger.LogInformation("Token refreshed successfully.");
                        return true;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Token refresh failed with status code: {response.StatusCode}. Response: {errorContent}");

                    // If refresh token is invalid, clear session and redirect to login
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                        response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await ClearSessionAndRedirectAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during token refresh.");
                await ClearSessionAndRedirectAsync();
            }

            return false;
        }

        private AuthResponse GetAuthResponseFromSession()
        {
            try
            {
                var authResponseJson = _contextAccessor.HttpContext?.Session.GetString("AuthResponse");
                if (!string.IsNullOrWhiteSpace(authResponseJson))
                {
                    return JsonConvert.DeserializeObject<AuthResponse>(authResponseJson);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing AuthResponse from session.");
            }
            return null;
        }

        private void SetAuthResponseInSession(AuthResponse authResponse)
        {
            try
            {
                var json = JsonConvert.SerializeObject(authResponse);
                _contextAccessor.HttpContext?.Session.SetString("AuthResponse", json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error serializing AuthResponse to session.");
            }
        }

        private async Task ClearSessionAndRedirectAsync()
        {
            var context = _contextAccessor.HttpContext;
            if (context != null)
            {
                try
                {
                    // Clear session
                    context.Session.Clear();
                    context.Session.Remove("JwtToken");
                    context.Session.Remove("RefreshToken");
                    context.Session.Remove("AuthResponse");

                    // Clear cookies
                    foreach (var cookie in context.Request.Cookies.Keys)
                    {
                        context.Response.Cookies.Delete(cookie);
                    }

                    // Sign out
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Redirect to login page
                    context.Response.Redirect("/Account/Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during session clear and redirect.");
                }
            }
        }
    }
}