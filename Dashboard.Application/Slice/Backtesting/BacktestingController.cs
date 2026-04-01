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

        [HttpPost("upsertDailyTickerFromPolygon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpsertDailyTickerFromPolygon(IEnumerable<string> symbols)
        {
            var response = await _externalDataService.UpsertDailyTickerFromPolygon(symbols.ToArray());
            return Ok(response);
        }
    }
}
