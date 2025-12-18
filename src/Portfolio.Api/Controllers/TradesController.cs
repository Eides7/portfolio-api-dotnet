using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts;
using Portfolio.Application.Trades.CreateTrade;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("trades")]
    public sealed class TradesController : ControllerBase
    {
        private readonly CreateTradeHandler _handler;

        public TradesController(CreateTradeHandler handler)
        {
            _handler = handler;
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

    }
}
