namespace ProjektMirjan.DTO
{
    public class CurrencyRateTableDTO
    {
        public string Table {  get; set; }
        public string No { get; set; } 
        public DateTime EffectiveDate { get; set; }
        public List<CurrencyRateDTO> Rates { get; set; }

    }
}
