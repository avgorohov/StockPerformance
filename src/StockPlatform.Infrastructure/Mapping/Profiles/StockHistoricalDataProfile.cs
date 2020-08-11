using AutoMapper;

namespace StockPlatform.Infrastructure.Mapping.Profiles
{
    public class StockHistoricalDataProfile : Profile
    {
        public StockHistoricalDataProfile()
        {
            CreateMap<Data.Models.StockHistoricalData, Domain.Models.Dto.StockHistoricalDataItem>();
            CreateMap<Domain.Models.Dto.StockHistoricalDataItem, Data.Models.StockHistoricalData>();
        }
    }
}
