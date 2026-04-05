public interface IDailyTickerRepository
{
    Task UpsertDailyTickersAsync(IEnumerable<DailyTickerSummary> tickers);
}