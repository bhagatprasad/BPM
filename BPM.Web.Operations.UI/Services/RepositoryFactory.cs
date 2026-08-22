// Services/IRepositoryFactory.cs
using BPM.Web.Operations.UI.Helper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BPM.Web.Operations.UI.Services
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RepositoryFactory> _logger;

        public RepositoryFactory(HttpClientService httpClientService, ILogger<RepositoryFactory> logger = null)
        {
            _httpClient = httpClientService.GetHttpClient();
            _logger = logger;
        }

        public async Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string uri, TRequest entity = default)
        {
            var requestMessage = new HttpRequestMessage(method, uri);

            if (entity != null)
            {
                var content = new StringContent(
                    JsonConvert.SerializeObject(entity),
                    Encoding.UTF8,
                    "application/json"
                );
                requestMessage.Content = content;
            }

            var response = await _httpClient.SendAsync(requestMessage);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string uri)
        {
            var requestMessage = new HttpRequestMessage(method, uri);
            var response = await _httpClient.SendAsync(requestMessage);
            return await HandleResponse<TResponse>(response);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger?.LogError($"API Error: {response.StatusCode} - {error}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Session expired. Please login again.");
                }

                throw new Exception($"API Error: {response.StatusCode}\n{error}");
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}