using StockPlatform.Domain.Models.Dto;
using System;
using System.Threading.Tasks;

namespace StockPlatform.Domain.Interfaces
{
    public interface IStockHistoricalDataService
    {
        StockHistoricalData GetStockHistoricalData(string symbol, DateTime from, DateTime to);
        Task StoreStockHistoricalData(StockHistoricalData stockHistoricalData);
    }
}
