using StockPlatform.Data.Repositories;
using System;

namespace StockPlatform.Data.Models
{
    public class StockHistoricalData : Entity<int>
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}
