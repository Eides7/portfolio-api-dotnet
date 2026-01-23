using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Portfolio.Api.Contracts;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Trades.CreateTrade;
using Portfolio.Application.Trades.GetTrades;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("trades")]
    public sealed class TradesController : ControllerBase
    {
        private readonly CreateTradeHandler _handler;
        private readonly GetTradesHandler _getHandler;
        private readonly ICacheService _cache;

        public TradesController(CreateTradeHandler handler, 
            GetTradesHandler getHandler, ICacheService cache)
        {
            _handler = handler; throw new ArgumentNullException(nameof(handler));
            _getHandler = getHandler; throw new ArgumentNullException(nameof(getHandler));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTradeRequest request, CancellationToken ct)
        {
            var cmd = new CreateTradeCommand(
                request.Instrument,
                request.Quantity,
                request.Price,
                request.Side,
                request.TradeDateUtc
                );

            //Invalidation of cached impacted
            await _cache.RemoveAsync("portfolio:positions:v1", ct);
            await _cache.RemoveAsync("portfolio:pnl:v1", ct);

            return Created(string.Empty, null);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TradeResponse>>> GetAll(CancellationToken ct)
        {
            var trades = await _getHandler.HandleAsync(ct);

            var response = trades
                .Select(t => new TradeResponse(t.Id, t.Instrument, t.Quantity, t.Price, t.Side, t.TradeDateUtc))
                .ToList();

            return Ok(response);
        }

    }
}
