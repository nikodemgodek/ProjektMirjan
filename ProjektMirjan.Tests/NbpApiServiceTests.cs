using ProjektMirjan.Service;
using NUnit.Framework;
using Moq;
using RestSharp;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using ProjektMirjan.DTO;
using System.Runtime.InteropServices;
using System.Net;
using ProjektMirjan.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjektMirjan.Tests
{
    [TestFixture]
    public class NbpApiServiceTests
    {
        private Mock<INbpApiService> _mockApiService;

        [SetUp]
        public void SetUp()
        {
            _mockApiService = new Mock<INbpApiService>();
        }

        [Test]
        public async Task GetCurrencyRatesAsync_WithMultipleRateTables_ReturnsExpectedRates()
        {
            var mockCurrencyRates = new List<CurrencyRateTableDTO>
            {
                new CurrencyRateTableDTO
                {
                    Table = "A",
                    EffectiveDate = DateTime.Now,
                    No = "082/A/NBP/2025",
                    Rates = new List<CurrencyRateDTO>
                    {
                        new CurrencyRateDTO { Code = "USD", Currency = "Dolar amerykański", Mid = 4.21M },
                        new CurrencyRateDTO { Code = "EUR", Currency = "Euro", Mid = 4.68M }
                    }
                },

                new CurrencyRateTableDTO
                {
                    Table = "A",
                    EffectiveDate = DateTime.Now,
                    No = "085/A/NBP/2025",
                    Rates = new List<CurrencyRateDTO>
                    {
                        new CurrencyRateDTO { Code = "PLN", Currency = "Polski złoty nowy", Mid = 4.21M },
                        new CurrencyRateDTO { Code = "TRY", Currency = "Lira", Mid = 4.68M }
                    }
                }
            };
            
            _mockApiService.Setup(service => service.GetCurrencyRatesAsync(null, null))
                .ReturnsAsync(mockCurrencyRates);

            var _nbpApiService = _mockApiService.Object;
            var result = await _nbpApiService.GetCurrencyRatesAsync(null, null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));

            Assert.That(result[0].Rates[0].Mid, Is.EqualTo(4.21M));
            Assert.That(result[0].Rates[0].Code, Is.EqualTo("USD"));
            Assert.That(result[0].Rates[0].Currency, Is.EqualTo("Dolar amerykański"));
            Assert.That(result[0].Rates[1].Mid, Is.EqualTo(4.68M));
            Assert.That(result[0].Rates[1].Code, Is.EqualTo("EUR"));
            Assert.That(result[0].Rates[1].Currency, Is.EqualTo("Euro"));

            Assert.That(result[1].Rates[0].Mid, Is.EqualTo(4.21M));
            Assert.That(result[1].Rates[0].Code, Is.EqualTo("PLN"));
            Assert.That(result[1].Rates[0].Currency, Is.EqualTo("Polski złoty nowy"));
            Assert.That(result[1].Rates[1].Mid, Is.EqualTo(4.68M));
            Assert.That(result[1].Rates[1].Code, Is.EqualTo("TRY"));
            Assert.That(result[1].Rates[1].Currency, Is.EqualTo("Lira"));
        }

        [Test]
        public void GetCurrencyRatesAsync_ThrowsInvalidOperationException_WhenStatusCodeIs400()
        {
            _mockApiService.Setup(service => service.GetCurrencyRatesAsync(null, null))
                .ThrowsAsync(new InvalidOperationException("Błąd 400: Nieprawidłowe żądanie."));

            var _nbpApiService = _mockApiService.Object;

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _nbpApiService.GetCurrencyRatesAsync(null, null));

            Assert.That(ex.Message, Does.Contain("400"));
            Assert.That(ex, Is.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GetCurrencyRatesAsync_ThrowsInvalidOperationException_WhenStatusCodeIs404()
        {
            _mockApiService.Setup(service => service.GetCurrencyRatesAsync(null, null))
                .ThrowsAsync(new InvalidOperationException("Błąd 404: Nie znaleziono."));

            var _nbpApiService = _mockApiService.Object;

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _nbpApiService.GetCurrencyRatesAsync(null, null));

            Assert.That(ex.Message, Does.Contain("404"));
            Assert.That(ex, Is.TypeOf<InvalidOperationException>());
        }
    }
}
