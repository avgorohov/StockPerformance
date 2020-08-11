using StockPlatform.Domain.Models;
using StockPlatform.Domain.Models.Dto;
using System;
using System.Collections.Generic;

namespace StockPlatform.Domain.Interfaces
{
    public interface IStockRetriever
    {
        public StockHistoricalData GetStockHistoricalData(string symbol, DateTime from, DateTime to, string apiKey);
    }

    public interface IStockPerformanceCalculator
    {
        public StockPerformance GetStockPerformance(StockHistoricalData historicalData);
        public IEnumerable<StockPerformance> GetStockPerformanceComparison(params StockHistoricalData[] historicalDataset);
    }
}
