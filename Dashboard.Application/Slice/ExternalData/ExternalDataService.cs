using System.Collections.Concurrent;
using YahooFinanceApi;

namespace Dashboard.Application.Slice.ExternalData
{
    public class ExternalDataService
    {
        public async Task<string> GetAndUpdateDataFromYFinance(string[] symbols)
        {
            // check existing symbols in database and only fetch missing ones
            // logic....

            var allData = new ConcurrentBag<(string Symbol, IEnumerable<YahooFinanceApi.Candle> Data)>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = 5 }; // throttle to avoid hitting API limits

            await Parallel.ForEachAsync(symbols, options, async (symbol, token) =>
            {
                try
                {
                    await Task.Delay(Random.Shared.Next(100, 300), token);
                    var history = await Yahoo.GetHistoricalAsync(symbol, new DateTime(1970, 1, 1), DateTime.Now);
                    allData.Add((symbol, history));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading {symbol}: {ex.Message}");
                }
            });


            //await SaveToDatabaseAsync(allData);

            return "test";
        }
    }
}
