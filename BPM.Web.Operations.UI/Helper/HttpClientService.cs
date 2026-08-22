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

        public HttpClientService(
            IOptions<BPMConfig> bpmConfig,
            IHttpClientFactory httpClientFactory,
            SessionManager sessionManager)
        {
            _sessionManager = sessionManager;

            // Create client without the handler
            _httpClient = httpClientFactory.CreateClient();

            var config = bpmConfig.Value;
            _httpClient.BaseAddress = new Uri(config.BaseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

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
        }

        public HttpClient GetHttpClient()
        {
            // Refresh authorization header before each use
            AddAuthorizationHeader();
            return _httpClient;
        }

        // Method to update token when it changes
        public void UpdateToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}