using ProjektMirjan.DTO;

namespace ProjektMirjan.Interfaces
{
    public interface INbpApiService
    {
        Task<List<CurrencyRateTableDTO>> GetCurrencyRatesAsync(DateTime? startDate, DateTime? endDate);
    }
}