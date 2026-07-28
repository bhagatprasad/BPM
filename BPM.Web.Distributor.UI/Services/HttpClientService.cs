using BPM.Web.Distributor.UI.Helpers;
using Microsoft.Extensions.Options;

namespace BPM.Web.Distributor.UI.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(
            IOptions<BPMConfig> config,
            IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("AuthorizedClient");

            _httpClient.BaseAddress = new Uri(config.Value.BaseUrl);

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
