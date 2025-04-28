using ProjektMirjan.Model;
using RestSharp;
using Newtonsoft.Json;
using ProjektMirjan.DTO;

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

        public async Task<CurrencyRateTableDTO> GetCurrencyRatesAsync()
        {
            var request = new RestRequest("exchangerates/tables/A?format=json", Method.Get);
            var response = await _client.ExecuteAsync(request);

            if(!response.IsSuccessful)
            {
                throw new Exception($"NBP API Error: {response.StatusCode} Response: {response.ErrorMessage}");
            }

            if(string.IsNullOrWhiteSpace(response.Content))
            {
                throw new Exception($"NBP API Error: No data fetched.");
            }

            try
            {
                var tables = JsonConvert.DeserializeObject<List<CurrencyRateTableDTO>>(response.Content);
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
