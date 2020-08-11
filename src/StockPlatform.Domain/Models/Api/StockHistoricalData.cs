using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StockPlatform.Domain.Models.Api
{
    internal class StockHistoricalData// : IStockHistoricalData
    {
        public StockHistoricalData()
        {
        }

        public StockHistoricalData(string symbol, IEnumerable<StockPrice> prices)
        {
            Symbol = symbol;
            Prices = prices;
        }

        [JsonPropertyName("prices")]
        public IEnumerable<StockPrice> Prices { get; set; } = new List<StockPrice>();
        public string Symbol { get; set; }
    }
}
