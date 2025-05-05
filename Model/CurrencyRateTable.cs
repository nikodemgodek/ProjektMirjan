using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProjektMirjan.Model
{
    public class CurrencyRateTable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Table {  get; set; }
        [Required]
        public string No {  get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        public List<CurrencyRate> CurrencyRates { get; set; }

    }
}
