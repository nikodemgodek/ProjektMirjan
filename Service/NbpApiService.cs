using ProjektMirjan.Model;
using RestSharp;
using Newtonsoft.Json;
using ProjektMirjan.DTO;
using Microsoft.IdentityModel.Tokens;
using ProjektMirjan.Interfaces;

namespace ProjektMirjan.Service
{
    public class NbpApiService : INbpApiService
    {
        private readonly RestClient _client;

        public NbpApiService(string baseUrl)
        {
            _client = new RestClient(baseUrl);
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
                        throw new InvalidOperationException("Błąd 400: Nieprawidłowe żądanie.");
                    case System.Net.HttpStatusCode.NotFound:
                        throw new InvalidOperationException("Błąd 404: Nie znaleziono.");
                    case System.Net.HttpStatusCode.InternalServerError:
                        throw new InvalidOperationException("Błąd 500: Problem z serwerem.");
                    case System.Net.HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException("Błąd 401: Brak autoryzacji.");
                    default:
                        throw new HttpRequestException($"Błąd API NBP: {response.StatusDescription}, kod: {response.StatusCode}");
                }
            }

            if(string.IsNullOrWhiteSpace(response.Content))
            {
                throw new Exception($"NBP API Error: API zwróciło pustą odpowiedź");
            }

            try
            {
                var tables = JsonConvert.DeserializeObject<List<CurrencyRateTableDTO>>(response.Content);

                if(tables == null)
                {
                    throw new Exception("NBP API Error: Pobrane dane są niepoprawne lub puste");
                }

                return tables.FirstOrDefault();
            }
            catch (JsonException ex)
            {
                throw new Exception("Błąd deserializacji", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Wystąpił nieoczekiwany błąd", ex);
            }
            
        }
    }
}
