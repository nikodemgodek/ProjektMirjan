using Newtonsoft.Json;

namespace ProjektMirjan.Model
{
    public class CurrencyRateTable
    {
        public int Id { get; set; }
        public string Table {  get; set; }
        public string No {  get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<CurrencyRate> CurrencyRates { get; set; }

    }
}
