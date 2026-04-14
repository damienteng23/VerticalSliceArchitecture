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

            var dataArray = symbolDataPastTwoYears.ToArray();
            var results = new List<MovingAveragePoints>();

            for (int i = 0; i < dataArray.Length; i++)
            {
                var calculatedPoint = new MovingAveragePoints
                {
                    Date = new DateTimeOffset(dataArray[i].From).ToUnixTimeSeconds(),
                    Close = dataArray[i].Close
                };

                var point = dataArray[i];
                bool hasShortPeriodBehind = (dataArray.Length - 1 - i) >= shortPeriod;
                bool hasLongPeriodBehind = (dataArray.Length - 1 - i) >= longPeriod;

                if (hasShortPeriodBehind)
                {
                    var shortPeriodPointData = dataArray.Skip(i ).Take(shortPeriod);
                    calculatedPoint.MaShortPeriod = shortPeriodPointData.Average(x => x.Close);
                }
                if (hasLongPeriodBehind)
                {
                    var longPeriodPointData = dataArray.Skip(i).Take(longPeriod);
                    calculatedPoint.MaLongPeriod = longPeriodPointData.Average(x => x.Close);
                }

                results.Add(calculatedPoint);
            }

            return results;
        }
    }
}
