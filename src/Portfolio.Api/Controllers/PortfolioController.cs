using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts;
using Portfolio.Application.Portfolio.GetPositions;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly GetPositionsHandler _handler;

        public PortfolioController(GetPositionsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("positions")]
        public async Task<ActionResult<IReadOnlyList<PositionResponse>>> GetPositions(CancellationToken ct)
        {
            var positions = await _handler.HandleAsync(ct);

            var response = positions
                .Select(p => new PositionResponse(p.Instrument, p.NetQuantity))
                .ToList();

            return Ok(response);
        }
    }
}
