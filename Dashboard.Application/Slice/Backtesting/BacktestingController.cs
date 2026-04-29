using Dashboard.Application.Slice.ExternalData;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dashboard.Application.Slice.Backtesting
{
    [Route("api/[controller]")]
    [ApiController]
    public class BacktestingController : Controller
    {
        private readonly IBacktestingService  _backtestingService;
        private readonly IExternalDataService _externalDataService;

        public BacktestingController(
            IBacktestingService backtestingService,
            IExternalDataService externalDataService
        )
        {
            _backtestingService = backtestingService;
            _externalDataService = externalDataService;
        }

        [HttpPost("upsertDailyTickerHistoricalDataFromPolygon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpsertDailyTickerHistoricalDataFromPolygon(string symbol)
        {
            var sp = new Stopwatch();
            sp.Restart();
            await _externalDataService.UpsertDailyTickerHistoricalDataFromPolygon(symbol);
            sp.Stop();
            Console.WriteLine($"Elapsed Time upsertDailyTickerHistoricalDataFromPolygon: {sp.Elapsed.Milliseconds}");
            return Ok();
        }

        [HttpGet("backtestMovingAverageStrategy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BacktestMovingAverageStrategy(string symbol, int shortPeriod, int longPeriod)
        {
            var sp = new Stopwatch();
            sp.Restart();
            var result = await _backtestingService.BacktestMovingAverageStrategy(symbol, shortPeriod, longPeriod);
            sp.Stop();
            Console.WriteLine($"Elapsed Time BacktestMovingAverageStrategy: {sp.Elapsed.Milliseconds}");
            return Ok(result);
        }

        [HttpGet("getDistinctTickers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistinctTickers()
        {
            var sp = new Stopwatch();
            sp.Restart();
            var result = await _backtestingService.GetDistinctTickers();
            sp.Stop();
            Console.WriteLine($"Elapsed Time GetDistinctTickers: {sp.Elapsed.Milliseconds}");
            return Ok(result);
        }
    }
}
