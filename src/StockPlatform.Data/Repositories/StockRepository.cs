using StockPlatform.Data.Interfaces;
using StockPlatform.Data.Models;

namespace StockPlatform.Data.Repositories
{
    public class StockRepository : EntityRepository<Stock>, IStockRepository
    {
        public StockRepository(StockComparisonDbContext context) : base(context) { }
    }
}
