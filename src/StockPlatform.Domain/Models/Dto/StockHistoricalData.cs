using System.Collections.Generic;

namespace StockPlatform.Domain.Models.Dto
{
    public class StockHistoricalData
    {
        public StockHistoricalData(string symbol)
        {
            Symbol = symbol;
        }

        public StockHistoricalData(string symbol, List<StockHistoricalDataItem> items)
        {
            Symbol = symbol;
            Items = items;
        }

        public string Symbol { get; private set; }
        public List<StockHistoricalDataItem> Items { get; private set; } = new List<StockHistoricalDataItem>();
    }
}
