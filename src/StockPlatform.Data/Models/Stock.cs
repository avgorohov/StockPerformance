using StockPlatform.Data.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockPlatform.Data.Models
{
    public class Stock : Entity<int>
    {
        public Stock()
        {
            StockHistoricalDatas = new HashSet<StockHistoricalData>();
        }

        [StringLength(20)]
        public string Symbol { get; set; }
        public ICollection<StockHistoricalData> StockHistoricalDatas { get; set; }
    }
}
