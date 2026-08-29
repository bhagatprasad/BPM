using BPM.Web.InventoryManagement.API.Models.DTOs;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BPM.Web.InventoryManagement.API.Integrations
{
    public class DistributorService : IDistributorService
    {
        private readonly HttpClient _httpClient;
       

        public DistributorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DistributorDto> GetDistributorByIdAsync(Guid distributorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Distributor/get-distributor-by-id/{distributorId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    var distributor = JsonSerializer.Deserialize<DistributorDto>(responseContent, options);
                    return distributor;
                }

                // Handle unsuccessful response
                throw new HttpRequestException($"Failed to get distributor. Status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}