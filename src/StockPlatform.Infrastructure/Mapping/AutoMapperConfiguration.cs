using AutoMapper;
using StockPlatform.Infrastructure.Mapping.Profiles;

namespace StockPlatform.Infrastructure.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StockProfile>();
                cfg.AddProfile<StockHistoricalDataProfile>();
            });
        }
    }

}
