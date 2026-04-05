using Dashboard.Application.Slice.Backtesting.Model;

namespace Dashboard.Application.Slice.Backtesting
{
    public interface IBacktestingService
    {
        Task<List<MovingAveragePoints>> BacktestMovingAverageStrategy(string symbol, int shortPeriod, int longPeriod);
    }
}
