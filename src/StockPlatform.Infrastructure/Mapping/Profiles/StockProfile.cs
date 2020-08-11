using AutoMapper;

namespace StockPlatform.Infrastructure.Mapping.Profiles
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<Data.Models.Stock, Domain.Models.Dto.Stock>();
            CreateMap<Domain.Models.Dto.Stock, Data.Models.Stock>();
        }
    }
}
