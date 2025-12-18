using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts;
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

        public TradesController(CreateTradeHandler handler, GetTradesHandler getHandler)
        {
            _handler = handler;
            _getHandler = getHandler;
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

            var result = await _handler.HandleAsync(cmd, ct);
            return Created($"/trades/{result.TradeId}", result);
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
