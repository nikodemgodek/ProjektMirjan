using Newtonsoft.Json;

namespace ProjektMirjan.Model
{
    public class CurrencyRateTable
    {
        public string Table {  get; set; }
        public string No {  get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<CurrencyRate> rates { get; set; }

    }
}
