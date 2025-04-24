using Microsoft.AspNetCore.Mvc;
using ProjektMirjan.Service;

namespace ProjektMirjan.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly NbpApiService _nbpApiService;
        private readonly ILogger<CurrencyRatesController> _logger;

        public CurrencyRatesController(NbpApiService nbpApiService, ILogger<CurrencyRatesController> logger)
        {
            _nbpApiService = nbpApiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates()
        {
            _logger.LogInformation("Starting request");

            var table = await _nbpApiService.GetCurrencyRatesAsync();

            if (table == null)
            {
                _logger.LogWarning("No exchange rates found or API error occurred.");
                return StatusCode(503, "API NBP is unavailable or returned an error.");
            }

            _logger.LogInformation("Exchange rates successfully fetched.");
            return Ok(table);
        }
    }
}
