using System;

namespace StockPlatform.Domain.Models
{
    public class DailyStockPerformance// : IDailyStockPerformance
    {
        public DailyStockPerformance()
        {
        }

        public DailyStockPerformance(DateTime dateTime, int value)
        {
            DateTime = dateTime;
            Value = value;
        }

        public DateTime DateTime { get; set; }
        public int Value { get; set; }
    }
}
