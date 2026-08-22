using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Helper
{
    public class TokenAuthorizationHandler : DelegatingHandler
    {
        private readonly SessionManager _sessionManager;
        private readonly IAuthenticateService _authService;
        private readonly ILogger<TokenAuthorizationHandler> _logger;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public TokenAuthorizationHandler(
            SessionManager sessionManager,
            IAuthenticateService authService,
            ILogger<TokenAuthorizationHandler> logger = null)
        {
            _sessionManager = sessionManager;
            _authService = authService;
            _logger = logger;

            // Set inner handler
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authResponse = _sessionManager.GetAuthResponse();

            if (authResponse != null && !string.IsNullOrWhiteSpace(authResponse.JwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.JwtToken);
                _logger?.LogDebug("Authorization header added to request.");
            }
            else
            {
                _logger?.LogWarning("No token available for request.");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger?.LogWarning("Received 401 Unauthorized. Attempting to refresh token.");

                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    var updatedAuthResponse = _sessionManager.GetAuthResponse();
                    if (updatedAuthResponse != null &&
                        !string.IsNullOrWhiteSpace(updatedAuthResponse.JwtToken) &&
                        updatedAuthResponse.JwtToken != authResponse?.JwtToken)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", updatedAuthResponse.JwtToken);
                        response = await base.SendAsync(request, cancellationToken);
                        return response;
                    }

                    var refreshed = await RefreshTokenAsync(cancellationToken);
                    if (refreshed)
                    {
                        var refreshedAuthResponse = _sessionManager.GetAuthResponse();
                        if (refreshedAuthResponse != null && !string.IsNullOrWhiteSpace(refreshedAuthResponse.JwtToken))
                        {
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedAuthResponse.JwtToken);
                            response = await base.SendAsync(request, cancellationToken);
                            _logger?.LogInformation("Token refreshed and request retried successfully.");
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error during token refresh.");
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
                var authResponse = _sessionManager.GetAuthResponse();
                if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.RefreshToken))
                {
                    _logger?.LogWarning("No refresh token available.");
                    _sessionManager.ClearSession();
                    return false;
                }

                var refreshRequest = new RefreshTokenRequestDto
                {
                    RefreshToken = authResponse.RefreshToken
                };

                var result = await _authService.RefreshTokenAsync(refreshRequest);

                if (result != null && !string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    // Update session
                    authResponse.JwtToken = result.AccessToken;
                    if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                    {
                        authResponse.RefreshToken = result.RefreshToken;
                    }
                    _sessionManager.SetAuthResponse(authResponse);
                    _sessionManager.SetToken(result.AccessToken, result.RefreshToken);

                    _logger?.LogInformation("Token refreshed successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception during token refresh.");
                _sessionManager.ClearSession();
            }

            return false;
        }
    }
}