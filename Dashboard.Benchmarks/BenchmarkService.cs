using BenchmarkDotNet.Attributes;
using Dashboard.Application.Slice.Backtesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dashboard.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 5)]
    public class BenchmarkService
    {
        private IBacktestingService _backtestingService;

        [GlobalSetup]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddLogging()
                .AddScoped<IDailyTickerRepository, DailyTickerRepository>()
                .AddScoped<IBacktestingService, BacktestingService>()
                .BuildServiceProvider();

            _backtestingService = serviceProvider.GetRequiredService<IBacktestingService>();
        }

        [Benchmark]
        public async Task RunBenchmarking()
        {
            await _backtestingService.BacktestMovingAverageStrategy("AAPL", 50, 200);
        }
    }
}
