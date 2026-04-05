using Dashboard.Application.Slice.ExternalData;
using Microsoft.AspNetCore.Mvc;

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
            await _externalDataService.UpsertDailyTickerHistoricalDataFromPolygon(symbol);
            return Ok();
        }

        [HttpPost("backtestMovingAverageStrategy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BacktestMovingAverageStrategy(string symbol, int shortPeriod, int longPeriod)
        {
            var result = await _backtestingService.BacktestMovingAverageStrategy(symbol, shortPeriod, longPeriod);
            return Ok(result);
        }

    }
}
