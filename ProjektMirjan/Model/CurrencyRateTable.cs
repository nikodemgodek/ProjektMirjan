using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProjektMirjan.Model
{
    public class CurrencyRateTable
    {
        [Key]
        public int Id { get; set; }
        public string Table {  get; set; }
        public string No {  get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<CurrencyRate> CurrencyRates { get; set; }

    }
}