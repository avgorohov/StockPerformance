using System.Collections.Generic;

namespace StockPlatform.Domain.Models
{
    public class StockPerformance// : IStockPerformance
    {
        //public StockPerformance()
        //{
        //}

        public StockPerformance(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; set; }
        public IList<DailyStockPerformance> DailyStockPerformances { get; set; } = new List<DailyStockPerformance>();
    }
}
