using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockPlatform.Api.Settings;
using StockPlatform.Domain.Interfaces;
using StockPlatform.Domain.Models.Dto;

namespace StockPlatform.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockRetriever _stockRetriever;
        private readonly IStockPerformanceCalculator _stockPerformanceCalculator;
        private readonly IStockHistoricalDataService _stockHistoricalDataService;
        private readonly AppSettings _appSettings;

        private const string SymbolToCompareWith = "SPY";

        public StocksController(IStockRetriever stockRetriever, IStockPerformanceCalculator stockPerformanceCalculator,
            IStockHistoricalDataService stockHistoricalDataService, IOptions<AppSettings> appSettings)
        {
            _stockRetriever = stockRetriever;
            _stockPerformanceCalculator = stockPerformanceCalculator;
            _stockHistoricalDataService = stockHistoricalDataService;
            _appSettings = appSettings.Value;
        }

        [HttpGet("{symbol}/comparison")]
        public async Task<IActionResult> GetSymbolComparison(string symbol)
        {
            var now = DateTime.Now;
            var from = now.AddDays(-7).Date;
            var to = now.Date;

            var requestedStockHistory = await RetrieveStocksHistoricalData(symbol, from, to);
            var comparingStockHistory = await RetrieveStocksHistoricalData(SymbolToCompareWith, from, to);

            var comparison = _stockPerformanceCalculator.GetStockPerformanceComparison(requestedStockHistory, comparingStockHistory);
            return Ok(comparison);
        }

        private async Task<StockHistoricalData> RetrieveStocksHistoricalData(string symbol, DateTime from, DateTime to)
        {
            var localHistoricalData = _stockHistoricalDataService.GetStockHistoricalData(symbol, from, to);
            var missingDates = CheckMissingDates(localHistoricalData, from, to);

            if (missingDates.Count > 0)
            {
                var missedFrom = missingDates.Min();
                var missedTo = missingDates.Max();
                var apiHistoricalData = _stockRetriever.GetStockHistoricalData(symbol, missedFrom, missedTo, _appSettings.StockIntegrationApiKey);
                var missingDatesApiHistoricalData = apiHistoricalData.Items.Where(p => missingDates.Contains(p.Date.Date));

                var historicalDataItems = new List<StockHistoricalDataItem>();
                foreach (var missingDateApiHistoricalData in missingDatesApiHistoricalData)
                {
                    var historicalDataDto = new StockHistoricalDataItem
                    {
                        Date = missingDateApiHistoricalData.Date,
                        Price = missingDateApiHistoricalData.Price
                    };

                    historicalDataItems.Add(historicalDataDto);
                    localHistoricalData.Items.Add(historicalDataDto);
                }

                await _stockHistoricalDataService.StoreStockHistoricalData(new StockHistoricalData(symbol, historicalDataItems));
            }

            return localHistoricalData;
        }

        private List<DateTime> CheckMissingDates(StockHistoricalData requestedHistoricalData, DateTime from, DateTime to)
        {
            var filledDates = requestedHistoricalData?.Items.ToDictionary(d => d.Date, d => d);
            var missedDates = new List<DateTime>();
            var dayoffs = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

            while (from <= to)
            {
                if (!filledDates.ContainsKey(from) && !dayoffs.Contains(from.DayOfWeek))
                {
                    missedDates.Add(from);
                }

                from = from.AddDays(1);
            }

            return missedDates;
        }
    }
}
