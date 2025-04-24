using ProjektMirjan.Model;
using RestSharp;
using System.Text.Json;
using Newtonsoft.Json;

namespace ProjektMirjan.Service
{
    public class NbpApiService
    {
        private readonly RestClient _client;
        private readonly ILogger<NbpApiService> _logger;

        public NbpApiService(ILogger<NbpApiService> logger)
        {
            _client = new RestClient("https://api.nbp.pl/api/");
            _logger = logger;
        }

        public async Task<CurrencyRateTable?> GetCurrencyRatesAsync()
        {
            var request = new RestRequest("exchangerates/tables/A?format=json", Method.Get);
            var response = await _client.ExecuteAsync(request);

            if(!response.IsSuccessful)
            {
                _logger.LogError($"Error: {response.StatusCode} - {response.ErrorMessage}");
            }

            if(string.IsNullOrWhiteSpace(response.Content))
            {
                _logger.LogWarning("Brak danych pobranych z API");
            }

            Console.WriteLine("RAW RESPONSE:");
            Console.WriteLine(response.Content);

            try
            {
                var tables = JsonConvert.DeserializeObject<List<CurrencyRateTable>>(response.Content);
                _logger.LogInformation(response.Content.ToString());
                return tables?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deserialization error: {ex.Message}");
                return null;
            }
        }
    }
}
