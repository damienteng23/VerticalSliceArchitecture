using Dashboard.Application.Slice.Backtesting.Model;

namespace Dashboard.Application.Slice.Backtesting
{
    public class BacktestingService : IBacktestingService
    {
        private readonly IDailyTickerRepository _dailyTickerRepository;
        public BacktestingService(IDailyTickerRepository dailyTickerRepository)
        {
            _dailyTickerRepository = dailyTickerRepository;
        }

        public async Task<List<MovingAveragePoints>> BacktestMovingAverageStrategy(string symbol, int shortPeriod, int longPeriod)
        {
            var symbolDataPastTwoYears = await _dailyTickerRepository
                .GetDailyTickerAsync(symbol, DateTime.Now.AddYears(-2), DateTime.Now);
            if (symbolDataPastTwoYears is null || !symbolDataPastTwoYears.Any())
                throw new ArgumentNullException($"GetDailyTickerAsync: No data found for the given symbol and date range.");

            var sorted = symbolDataPastTwoYears.OrderByDescending(x => x.From).ToArray();
            var results = new List<MovingAveragePoints>();

            for (int i = 0; i < sorted.Length; i++)
            {
                var calculatedPoint = new MovingAveragePoints
                {
                    Date = sorted[i].From.Date.ToString("yyyy-MM-dd"),
                    Close = sorted[i].Close
                };

                var point = sorted[i];
                bool hasShortPeriodBehind = (sorted.Length - 1 - i) >= shortPeriod;
                bool hasLongPeriodBehind = (sorted.Length - 1 - i) >= longPeriod;

                if (hasShortPeriodBehind)
                {
                    var shortPeriodPointData = sorted.Skip(i ).Take(shortPeriod);
                    calculatedPoint.maShortPeriod = shortPeriodPointData.Average(x => x.Close);
                }
                if (hasLongPeriodBehind)
                {
                    var longPeriodPointData = sorted.Skip(i).Take(longPeriod);
                    calculatedPoint.maLongPeriod = longPeriodPointData.Average(x => x.Close);
                }

                results.Add(calculatedPoint);
            }

            return results;
        }
    }
}
