using NUnit.Framework;
using Moq;
using ProjektMirjan.Service;
using ProjektMirjan.Context;
using ProjektMirjan.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ProjektMirjan.DTO;
using ProjektMirjan.Interfaces;
using System.Text.Json;

namespace ProjektMirjan.Tests
{
    public class CurrencyRateServiceTests
    {
        private CurrencyRateService _currencyRateService;
        private CurrencyContext _currencyContext;


        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CurrencyContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRateTestDB")
                .Options;

            _currencyContext = new CurrencyContext(options);
            _currencyRateService = new CurrencyRateService(_currencyContext);

        }

        [TearDown]
        public void TearDown()
        {
            _currencyContext.Database.EnsureDeleted();
            _currencyContext.Dispose();
        }

        [Test]
        public async Task SaveCurrencyRatesAsync_WhenCurrencyRatesAreNotExist_ShouldSaveData()
        {
            var tableDTO = new CurrencyRateTableDTO
            {
                Table = "A",
                No = "123",
                EffectiveDate = DateTime.Now,
                Rates = new List<CurrencyRateDTO>
                {
                    new CurrencyRateDTO { Currency = "USD", Code = "USD", Mid = 3.75m }
                }
            };

            await _currencyRateService.SaveCurrencyRatesAsync(new List<CurrencyRateTableDTO> { tableDTO });

            var savedTable = _currencyContext.CurrencyRateTables.Include(t => t.CurrencyRates).FirstOrDefault(t => t.Table == tableDTO.Table && t.No == tableDTO.No);

            Assert.That(savedTable, Is.Not.Null);
            Assert.That(savedTable.CurrencyRates.Count, Is.EqualTo(1));
            Assert.That(savedTable.CurrencyRates[0].Currency, Is.EqualTo("USD"));
        }

        [Test]
        public async Task SaveCurrencyRatesAsync_NotDuplicateExistingTable()
        {
            var tableDTO = new CurrencyRateTableDTO
            {
                Table = "A",
                No = "123",
                EffectiveDate = DateTime.Now,
                Rates = new List<CurrencyRateDTO>
                {
                    new CurrencyRateDTO { Currency = "USD", Code = "USD", Mid = 3.75m }
                }
            };

            await _currencyRateService.SaveCurrencyRatesAsync(new List<CurrencyRateTableDTO> { tableDTO });
            await _currencyRateService.SaveCurrencyRatesAsync(new List<CurrencyRateTableDTO> { tableDTO });

            var tables = _currencyContext.CurrencyRateTables.Include(t => t.CurrencyRates).ToList();
            Assert.That(tables.Count, Is.EqualTo(1));
        }

        [Test]
        public void CurrencyRateTableDTO_ShouldSerializeCorrectlyToJson()
        {
            var dto = new CurrencyRateTableDTO
            {
                Table = "A",
                No = "123",
                EffectiveDate = new DateTime(2025, 5, 1),
                Rates = new List<CurrencyRateDTO>
            {
                new CurrencyRateDTO { Currency = "Euro", Code = "EUR", Mid = 4.56m },
                new CurrencyRateDTO { Currency = "Dolar amerykañski", Code = "USD", Mid = 3.75m }
            }
            };

            var json = JsonSerializer.Serialize(dto);

            Assert.That(json, Does.Contain("\"Table\":\"A\""));
            Assert.That(json, Does.Contain("\"Code\":\"EUR\""));
            Assert.That(json, Does.Contain("\"Mid\":4.56"));
        }

        [Test]
        public void SaveCurrencyRatesAsync_WhenDataIsInvalid_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<CurrencyContext>()
                .UseInMemoryDatabase("TestDbError")
                .Options;

            var context = new CurrencyContext(options);

            var service = new CurrencyRateService(context);

            var invalidDto = new CurrencyRateTableDTO
            {
                Table = null,
                No = "123",
                EffectiveDate = DateTime.Now,
                Rates = new List<CurrencyRateDTO>()
            };

            Assert.ThrowsAsync<DbUpdateException>(async () => await service.SaveCurrencyRatesAsync(new List<CurrencyRateTableDTO> { invalidDto }));
        }
    }
}