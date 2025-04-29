using ProjektMirjan.Model;
using RestSharp;
using Newtonsoft.Json;
using ProjektMirjan.DTO;

namespace ProjektMirjan.Service
{
    public class NbpApiService
    {
        private readonly RestClient _client;

        public NbpApiService(string apiUrl = "https://api.nbp.pl/api/")
        {
            _client = new RestClient(apiUrl);
        }

        public async Task<CurrencyRateTableDTO> GetCurrencyRatesAsync()
        {
            
            var request = new RestRequest("exchangerates/tables/A?format=json", Method.Get);
            var response = await _client.ExecuteAsync(request);

            if(!response.IsSuccessful)
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        throw new InvalidOperationException("Błąd 400: Nieprawidłowe żądanie");
                    case System.Net.HttpStatusCode.NotFound:
                        throw new InvalidOperationException("Błąd 404: Nie znaleziono");
                    default:
                        throw new HttpRequestException($"Błąd API NBP: {response.StatusDescription}");
                }
            }

            if(string.IsNullOrWhiteSpace(response.Content))
            {
                throw new Exception($"NBP API Error: API zwróciło pustą odpowiedź");
            }

            try
            {
                var tables = JsonConvert.DeserializeObject<List<CurrencyRateTableDTO>>(response.Content);
                return tables?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
