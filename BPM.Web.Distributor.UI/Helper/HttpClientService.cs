using Microsoft.Extensions.Options;

namespace BPM.Web.Distributor.UI.Helpers
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(
            IOptions<BPMConfig> bpmConfig,
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthorizedClient");

            var config = bpmConfig.Value;

            _httpClient.BaseAddress = new Uri(config.BaseUrl);

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}