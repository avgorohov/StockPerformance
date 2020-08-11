using StockPlatform.Data.Interfaces;
using StockPlatform.Data.Models;

namespace StockPlatform.Data.Repositories
{
    public class StockHistoricalDataRepository : EntityRepository<StockHistoricalData>, IStockHistoricalDataRepository
    {
        public StockHistoricalDataRepository(StockComparisonDbContext context) : base(context) { }
    }
}
