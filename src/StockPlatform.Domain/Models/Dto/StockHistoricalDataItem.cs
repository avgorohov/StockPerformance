using System;

namespace StockPlatform.Domain.Models.Dto
{
    public class StockHistoricalDataItem
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
