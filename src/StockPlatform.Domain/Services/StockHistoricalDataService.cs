using AutoMapper;
using StockPlatform.Data.Interfaces;
using StockPlatform.Domain.Interfaces;
using StockPlatform.Domain.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockPlatform.Domain.Services
{
    public class StockHistoricalDataService : IStockHistoricalDataService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockHistoricalDataRepository _stockHistoricalDataRepository;
        private readonly IMapper _mapper;

        public StockHistoricalDataService(IStockRepository stockRepository, IStockHistoricalDataRepository stockHistoricalDataRepository, 
            IMapper mapper)
        {
            _stockRepository = stockRepository;
            _stockHistoricalDataRepository = stockHistoricalDataRepository;
            _mapper = mapper;
        }

        public StockHistoricalData GetStockHistoricalData(string symbol, DateTime from, DateTime to)
        {
            var entities = _stockHistoricalDataRepository.GetAll(e => e.Stock.Symbol == symbol && e.Date >= from && e.Date <= to, 
                orderBy: e => e.OrderBy(x => x.Date),
                includeProperties: "Stock");

            var items = _mapper.Map<List<StockHistoricalDataItem>>(entities);
            return new StockHistoricalData(symbol, items);
        }

        public async Task StoreStockHistoricalData(StockHistoricalData stockHistoricalData)
        {
            if (stockHistoricalData == null || stockHistoricalData.Items == null)
            {
                throw new ArgumentException("stockHistoricalData should contain at least one item");
            }

            var stock = _stockRepository.GetOne(e => e.Symbol == stockHistoricalData.Symbol);
            if (stock == null)
            {
                stock = new Data.Models.Stock
                {
                    Symbol = stockHistoricalData.Symbol
                };

                
                await _stockRepository.CreateAsync(stock);
                await _stockRepository.SaveAsync();
            }

            foreach (var stockHistoricalDataItem in stockHistoricalData.Items)
            {
                await _stockHistoricalDataRepository.CreateAsync(new Data.Models.StockHistoricalData
                {
                    Date = stockHistoricalDataItem.Date,
                    Price = stockHistoricalDataItem.Price,
                    StockId = stock.Id
                });
            }

            await _stockRepository.SaveAsync();
        }
    }
}
