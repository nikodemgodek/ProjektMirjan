namespace ProjektMirjan.Model
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }

        public int CurrencyRateTableId {  get; set; }
        public CurrencyRateTable CurrencyRateTable { get; set; }
    }
}
