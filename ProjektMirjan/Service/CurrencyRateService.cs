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

        public async Task SaveCurrencyRatesAsync(List<CurrencyRateTableDTO> tables)
        {

            foreach (var table in tables)
            {
                var exists = _currencyContext.CurrencyRateTables.Any(t => t.Table == table.Table && t.No == table.No);

                if(exists)
                {
                    continue;
                }

                var newTable = new CurrencyRateTable
                {
                    Table = table.Table,
                    No = table.No,
                    EffectiveDate = table.EffectiveDate,
                    CurrencyRates = table.Rates.Select(r => new CurrencyRate
                    {
                        Currency = r.Currency,
                        Code = r.Code,
                        Mid = r.Mid
                    }).ToList()
                };

                await _currencyContext.CurrencyRateTables.AddAsync(newTable);
            }
            
            await _currencyContext.SaveChangesAsync();
        }
    }
}
