using ProjektMirjan.Model;
using RestSharp;
using Newtonsoft.Json;
using ProjektMirjan.DTO;
using Microsoft.IdentityModel.Tokens;
using ProjektMirjan.Interfaces;
using System.Runtime.CompilerServices;

namespace ProjektMirjan.Service
{
    public class NbpApiService : INbpApiService
    {
        private readonly RestClient _client;

        public NbpApiService(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public async Task<List<CurrencyRateTableDTO>> GetCurrencyRatesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            if(startDate.HasValue && endDate.HasValue && (endDate.Value - startDate.Value).Days > 93)
            {
                throw new ArgumentException("Przekroczono zakres 93 dni");
            }

            string apiUrl;
            if (!startDate.HasValue || !endDate.HasValue)
            {
                apiUrl = $"exchangerates/tables/A?format=json";
            } else
            {
                string formatStart = startDate.Value.ToString("yyyy-MM-dd");
                string formatEnd = endDate.Value.ToString("yyyy-MM-dd");
                apiUrl = $"exchangerates/tables/A/{formatStart}/{formatEnd}?format=json";
            }

            var request = new RestRequest(apiUrl, Method.Get);
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

                return tables;
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
