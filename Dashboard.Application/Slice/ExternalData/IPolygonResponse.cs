using Refit;

namespace Dashboard.Application.Slice.ExternalData
{
    public interface IPolygonApi
    {
        [Get("/v2/aggs/ticker/{symbol}/range/1/day/{startDate}/{endDate}")]
        Task<PolygonResponse> GetDailyAggregates(
            string symbol,
            string startDate,
            string endDate,
            [AliasAs("adjusted")] bool adjusted = true,
            [AliasAs("sort")] string sort = "asc",
            [AliasAs("limit")] int limit = 50000,
            [AliasAs("apiKey")] string apiKey = "YOUR_KEY"
        );
    }
}
