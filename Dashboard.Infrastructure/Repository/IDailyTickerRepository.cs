public interface IDailyTickerRepository
{
    Task<IEnumerable<DailyTickerSummary>> GetDailyTickerAsync(string symbol, DateTime startDate, DateTime endDate);
    Task<DateTime?> GetLatestDateAsync(string symbol);
    Task<bool> TickerExistsAsync(string symbol);
    Task UpsertDailyTickersAsync(IEnumerable<DailyTickerSummary> tickers);
}