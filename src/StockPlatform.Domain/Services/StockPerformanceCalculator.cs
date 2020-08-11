using StockPlatform.Domain.Interfaces;
using StockPlatform.Domain.Models;
using StockPlatform.Domain.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockPlatform.Domain.Services
{
    public class StockPerformanceCalculator : IStockPerformanceCalculator
    {
        public StockPerformance GetStockPerformance(StockHistoricalData historicalData)
        {
            const int initialPerformanceValue = 0;

            var performance = new StockPerformance(historicalData.Symbol);

            if (historicalData == null || historicalData.Items == null || !historicalData.Items.Any()) return performance;

            var prices = historicalData.Items.OrderBy(p => p.Date).ToList();
            var initialPerformancePrice = prices[0].Price;
            performance.DailyStockPerformances.Add(new DailyStockPerformance(prices[0].Date, initialPerformanceValue));

            if (prices.Count > 1)
            {
                for (int i = 1; i < prices.Count; i++)
                {
                    var performanceValue = CalculatePerformanceValue(prices[i].Price, initialPerformancePrice);
                    performance.DailyStockPerformances.Add(new DailyStockPerformance(prices[i].Date, performanceValue));
                }
            }


            return performance;
        }

        public IEnumerable<StockPerformance> GetStockPerformanceComparison(params StockHistoricalData[] historicalDataset)
        {
            var result = new List<StockPerformance>();
            foreach (var historicalData in historicalDataset)
            {
                result.Add(GetStockPerformance(historicalData));
            }

            return result;
        }

        private int CalculatePerformanceValue(decimal price, decimal initialPerformancePrice)
        {
            if (initialPerformancePrice == 0) throw new ArgumentException("Initial price value couldn't have 0 value");

            var value = (int)((price - initialPerformancePrice) * 100 / initialPerformancePrice);
            return value;
        }
    }
}
