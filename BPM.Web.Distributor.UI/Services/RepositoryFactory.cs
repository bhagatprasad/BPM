using BPM.Web.Distributor.UI.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BPM.Web.Distributor.UI.Services
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly HttpClient _httpClient;

        public RepositoryFactory(HttpClientService httpClientService)
        {
            _httpClient = httpClientService.GetHttpClient();
        }

        public async Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string uri, TRequest entity = default)
        {
            var requestMessage = new HttpRequestMessage(method, uri);

            if (entity != null)
            {
                var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
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

        //private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        //{
        //    if (response.IsSuccessStatusCode)
        //    {
        //        if (typeof(T) == typeof(bool) && response.StatusCode == HttpStatusCode.NoContent)
        //        {
        //            return (T)(object)true;
        //        }

        //        var content = await response.Content.ReadAsStringAsync();
        //        var olcResponse = JsonConvert.DeserializeObject<T>(content);

        //        return olcResponse;

        //    }
        //    return default;
        //}

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Status: {response.StatusCode}\n{error}"
                );
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
