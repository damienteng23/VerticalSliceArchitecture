using Dashboard.Domain.Entities;
using System.Collections.Concurrent;

namespace Dashboard.Application.Slice.ExternalData
{
    public class ExternalDataService : IExternalDataService
    {
        private readonly string _apiKeyMassive;
        private readonly IPolygonApi _polygonApi;
        private readonly IDailyTickerRepository _dailyTickerRepository;

        public ExternalDataService(IConfiguration configuration, IPolygonApi polygonApi, IDailyTickerRepository dailyTickerRepository)
        {
            _apiKeyMassive = configuration["ApiKeys:hy_massive_api_key"];
            _polygonApi = polygonApi;
            _dailyTickerRepository = dailyTickerRepository;
        }

        public async Task UpsertDailyTickerHistoricalDataFromPolygon(string symbol)
        {
            var startDate = DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Call API
            var response = await _polygonApi.GetDailyAggregates(
                symbol, startDate, endDate, apiKey: _apiKeyMassive);

            // Map PolygonAgg → DailyTicker
            var tickers = response.Results
                .Select(agg => new DailyTickerSummary
                (
                    symbol,
                    UnixTimeStampToDateTime(agg.Timestamp),
                    (decimal)agg.Open,
                    (decimal)agg.High,
                    (decimal)agg.Low,
                    (decimal)agg.Close,
                    (long)agg.Volume,
                    0m,  // Polygon doesn't provide this in aggregates endpoint
                    0m, // Polygon doesn't provide this in aggregates endpoint
                    "active"
                ));

            // save to DB
            await _dailyTickerRepository.UpsertDailyTickersAsync(tickers);
        }

        private DateTime UnixTimeStampToDateTime(long timestamp)
            => DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
    }
}
