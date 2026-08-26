using System.Text.Json;
using BPM.Web.Orders.API.Models.DTOs;

namespace BPM.Web.Orders.API.Integrations
{
    public class DrugService : IDrugService
    {
        private readonly HttpClient _httpClient;

        public DrugService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResponseDrugDto>> GetAllDrugsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("drug/get-all-drugs");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var drugs = JsonSerializer.Deserialize<List<ResponseDrugDto>>(responseContent, options);

                    return drugs ?? new List<ResponseDrugDto>();
                }

                return new List<ResponseDrugDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<ResponseDrugDto>();
            }
        }
    }
}