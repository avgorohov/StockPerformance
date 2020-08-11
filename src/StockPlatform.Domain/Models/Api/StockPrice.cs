using System.Text.Json.Serialization;

namespace StockPlatform.Domain.Models.Api
{
    internal class StockPrice
    {
        [JsonPropertyName("date")]
        public long Date { get; set; }

        [JsonPropertyName("open")]
        public decimal Open { get; set; }

        [JsonPropertyName("high")]
        public decimal High { get; set; }

        [JsonPropertyName("low")]
        public decimal Low { get; set; }

        [JsonPropertyName("close")]
        public decimal Close { get; set; }

        [JsonPropertyName("adjclose")]
        public decimal Adjclose { get; set; }

        [JsonPropertyName("volume")]
        public int Volume { get; set; }
    }
}
