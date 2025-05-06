using Microsoft.AspNetCore.Mvc;
using ProjektMirjan.DTO;
using ProjektMirjan.Interfaces;
using ProjektMirjan.Service;

namespace ProjektMirjan.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly INbpApiService _nbpApiService;
        private readonly CurrencyRateService _currencyRateService;
        private readonly ILogger<CurrencyRatesController> _logger;

        public CurrencyRatesController(INbpApiService nbpApiService, ILogger<CurrencyRatesController> logger, CurrencyRateService currencyRateService)
        {
            _nbpApiService = nbpApiService;
            _currencyRateService = currencyRateService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            _logger.LogInformation("Starting request");

            var tables = await _nbpApiService.GetCurrencyRatesAsync(startDate, endDate);

            if (tables == null)
            {
                _logger.LogWarning("No exchange rates found or API error occurred.");
                return StatusCode(503, "API NBP is unavailable or returned an error.");
            }

            _logger.LogInformation("Exchange rates successfully fetched.");
            await _currencyRateService.SaveCurrencyRatesAsync(tables);
            return Ok(tables) ;
        }
    }
}
