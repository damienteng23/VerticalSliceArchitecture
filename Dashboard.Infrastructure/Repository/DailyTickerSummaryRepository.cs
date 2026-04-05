using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

public class DailyTickerRepository : IDailyTickerRepository
{
    private readonly string _connectionString;
    private readonly ILogger<DailyTickerRepository> _logger;

    public DailyTickerRepository(IConfiguration config, ILogger<DailyTickerRepository> logger)
    {
        _connectionString = config.GetConnectionString("DashboardDb")
            ?? throw new InvalidOperationException("Connection string not found");
        _logger = logger;
    }

    public async Task UpsertDailyTickersAsync(IEnumerable<DailyTickerSummary> tickers)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var dataTable = ToDataTable(tickers);

                // Create parameter for TVP
                var parameters = new DynamicParameters();
                parameters.Add("@DailyTickers", dataTable.AsTableValuedParameter("dbo.udtt_DailyTickerType"));

                // Execute stored procedure
                await connection.ExecuteAsync(
                    "sp_UpsertDailyTickers",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300); // 5 minutes for large batches

                _logger.LogInformation($"Successfully upserted {tickers.Count()} daily ticker records");
            }
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Database error while upserting daily tickers");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while upserting daily tickers");
            throw;
        }
    }

    /// <summary>
    /// Convert IEnumerable<DailyTickerSummary> to DataTable for TVP
    /// </summary>
    private DataTable ToDataTable(IEnumerable<DailyTickerSummary> tickers)
    {
        var dt = new DataTable();

        // Add columns matching your SQL type
        dt.Columns.Add("Symbol", typeof(string));
        dt.Columns.Add("Date", typeof(DateTime));
        dt.Columns.Add("Open", typeof(decimal));
        dt.Columns.Add("High", typeof(decimal));
        dt.Columns.Add("Low", typeof(decimal));
        dt.Columns.Add("Close", typeof(decimal));
        dt.Columns.Add("Volume", typeof(long));
        dt.Columns.Add("PreMarket", typeof(decimal));
        dt.Columns.Add("AfterHours", typeof(decimal));
        dt.Columns.Add("Status", typeof(string));

        // Add rows
        foreach (var ticker in tickers)
        {
            dt.Rows.Add(
                ticker.Symbol,
                ticker.From,
                ticker.Open,
                ticker.High,
                ticker.Low,
                ticker.Close,
                ticker.Volume,
                ticker.PreMarket,
                ticker.AfterHours,
                ticker.Status
            );
        }

        return dt;
    }

    /// <summary>
    /// Check if ticker data exists for a symbol
    /// </summary>
    public async Task<bool> TickerExistsAsync(string symbol)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = "SELECT COUNT(1) FROM [dbo].[DailyTicker] WHERE [Symbol] = @Symbol";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Symbol = symbol });
            return count > 0;
        }
    }

    /// <summary>
    /// Get the latest date for a symbol
    /// </summary>
    public async Task<DateTime?> GetLatestDateAsync(string symbol)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = "SELECT MAX([Date]) FROM [dbo].[DailyTicker] WHERE [Symbol] = @Symbol";
            var result = await connection.ExecuteScalarAsync<DateTime?>(sql, new { Symbol = symbol });
            return result;
        }
    }

    public async Task<IEnumerable<DailyTickerSummary>> GetDailyTickerAsync(string symbol, DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryAsync<DailyTickerSummary>(
                 @"SELECT * FROM [dbo].[DailyTickerSummary]
                  WHERE Symbol = @symbol
                  AND [From] >= @startDate
                  AND [From] <= @endDate
                  ORDER BY [From] ASC",
             new { symbol, startDate, endDate }
            );
        }
    }
}