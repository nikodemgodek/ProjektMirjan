using ProjektMirjan.Context;
using ProjektMirjan.DTO;
using ProjektMirjan.Model;

namespace ProjektMirjan.Service
{
    public class CurrencyRateService
    {
        public CurrencyContext _currencyContext;

        public CurrencyRateService(CurrencyContext currencyContext)
        {
            _currencyContext = currencyContext;
        }

        public async Task SaveCurrencyRatesAsync(CurrencyRateTableDTO tableDTO)
        {
            var existingTable = _currencyContext.CurrencyRateTables
                .FirstOrDefault(t => t.Table == tableDTO.Table && t.No == tableDTO.No);

            if(existingTable != null)
            {
                return;
            }

            var newTable = new CurrencyRateTable
            {
                Table = tableDTO.Table,
                No = tableDTO.No,
                EffectiveDate = tableDTO.EffectiveDate,
                CurrencyRates = tableDTO.Rates.Select(r => new CurrencyRate
                {
                    Currency = r.Currency,
                    Code = r.Code,
                    Mid = r.Mid
                }).ToList()
            };

            await _currencyContext.CurrencyRateTables.AddAsync(newTable);
            await _currencyContext.SaveChangesAsync();
        }
    }
}
