using BPM.Web.InventoryManagement.API.Models.DTOs;
using System.Text.Json;

namespace BPM.Web.InventoryManagement.API.Integrations
{
    public class DrugService : IDrugService
    {
        private readonly HttpClient _httpClient;

        public DrugService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DrugDto>> GetAllDrugsAsync()
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

                    var drugs = JsonSerializer.Deserialize<List<DrugDto>>(responseContent, options);

                    return drugs ?? new List<DrugDto>();
                }

                return new List<DrugDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<DrugDto>();
            }
        }
    }
}
