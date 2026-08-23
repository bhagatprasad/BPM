using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BPM.Web.Operations.UI.Helper
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly SessionManager _sessionManager;
        private readonly BPMConfig _config;

        public HttpClientService(
            IOptions<BPMConfig> bpmConfig,
            SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _config = bpmConfig.Value;

            // Create client WITHOUT the authorization handler
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config.BaseUrl),
                Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds)
            };

            _httpClient.DefaultRequestHeaders.Clear();

            // Add authorization header if token exists
            AddAuthorizationHeader();
        }

        private void AddAuthorizationHeader()
        {
            var authResponse = _sessionManager.GetAuthResponse();
            if (authResponse != null && !string.IsNullOrWhiteSpace(authResponse.JwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authResponse.JwtToken);
            }
            else
            {
                // Clear authorization header if no token
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public HttpClient GetHttpClient()
        {
            // Refresh authorization header before each use
            AddAuthorizationHeader();
            return _httpClient;
        }

        public void UpdateToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        // Force refresh of token
        public void RefreshAuthorizationHeader()
        {
            AddAuthorizationHeader();
        }
    }
}