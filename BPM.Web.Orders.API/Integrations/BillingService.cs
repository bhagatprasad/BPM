using BPM.Web.Orders.API.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace BPM.Web.Orders.API.Integrations
{
    public class BillingService : IBillingService
    {
        private readonly HttpClient _httpClient;

        public BillingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BillingResponseDto> CreateBillingAsync(CreateBillingDto createBillingDto, Guid currentUserId)
        {
            var content = new StringContent(JsonSerializer.Serialize(createBillingDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("billing/create-billing", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<BillingResponseDto>(responseContent, options);
            }

            return null;
        }

        public Task<IEnumerable<BillingResponseDto>> GetAllBillingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BillingResponseDto?> GetBillingByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BillingResponseDto?> GetBillingBySalesOrderIdAsync(Guid salesOrderId)
        {
            throw new NotImplementedException();
        }
    }
}
