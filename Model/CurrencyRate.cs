using System.ComponentModel.DataAnnotations;

namespace ProjektMirjan.Model
{
    public class CurrencyRate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        [MaxLength(3)]
        public string Code { get; set; }
        [Required]
        public decimal Mid { get; set; }

        public int CurrencyRateTableId {  get; set; }
        public CurrencyRateTable CurrencyRateTable { get; set; }
    }
}
