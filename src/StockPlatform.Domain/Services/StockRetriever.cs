using RestSharp;
using StockPlatform.Domain.Extensions;
using StockPlatform.Domain.Interfaces;
using StockPlatform.Domain.Models.Dto;
using System;
using System.Linq;

namespace StockPlatform.Domain.Services
{
    public class StockRetriever : IStockRetriever
    {
        public StockHistoricalData GetStockHistoricalData(string symbol, DateTime from, DateTime to, string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "Api Key shouldn't be null");
            }
            var periodFrom = from.ToEpochTime();
            var periodTo = to.ToEpochTime();
            var client = new RestClient($"https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/v2/get-historical-data?frequency=1d&filter=history&period1={periodFrom}&period2={periodTo}&symbol={symbol}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", apiKey);
            var response = client.Get<Models.Api.StockHistoricalData>(request);
            var data = response.Data;

            if (data == null) return new StockHistoricalData(symbol);

            return new StockHistoricalData(symbol, data.Prices.Where(e => e.Open != 0).Select(e => new StockHistoricalDataItem
            {
                Date = e.Date.ToDateTimeFromEpoch().Date,
                Price = e.Open
            }).ToList());
        }
    }
}
