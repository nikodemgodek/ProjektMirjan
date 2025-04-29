using ProjektMirjan.DTO;

namespace ProjektMirjan.Interfaces
{
    public interface INbpApiService
    {
        Task<CurrencyRateTableDTO> GetCurrencyRatesAsync();
    }
}
