using System.Collections.Concurrent;

namespace Dashboard.Application.Slice.ExternalData
{
    public class ExternalDataService : IExternalDataService
    {
        private readonly string _apiKeyMassive;
        private readonly HttpClient _httpClient;

        public ExternalDataService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKeyMassive = configuration["ApiKeys:hy_massive_api_key"];
            _httpClient = httpClient;
        }

        public async Task<string> UpsertDailyTickerFromPolygon(string[] symbols)
        {
            // check existing symbols in database and only fetch missing ones
            // logic....

            //var allData = new ConcurrentBag<(string Symbol, IEnumerable<YahooFinanceApi.Candle> Data)>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = 5 }; // throttle to avoid hitting API limits

            try
            {
                //var history = await Yahoo.GetHistoricalAsync(symbols[0], DateTime.Now.AddMonths(-1), DateTime.Now);
                //allData.Add((symbols[0], history));
            }
            catch (Exception ex)
            {
                // Log the actual response body to see if Yahoo gave a specific reason
                //var errorBody = await ex.GetResponseStringAsync();
                //Console.WriteLine($"Yahoo API Error: {errorBody}");
            }

            //await Parallel.ForEachAsync(symbols, options, async (symbol, token) =>
            //{
            //    try
            //    {
            //        await Task.Delay(Random.Shared.Next(100, 300), token);
            //        var history = await Yahoo.GetHistoricalAsync(symbol, new DateTime(1970, 1, 1), DateTime.Now);
            //        allData.Add((symbol, history));
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error downloading {symbol}: {ex.Message}");
            //    }
            //});


            //await SaveToDatabaseAsync(allData);

            return "test";
        }
    }
}
