using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Portfolio.GetPnL;
using Portfolio.Application.Portfolio.GetPositions;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly GetPositionsHandler _handler;
        private readonly GetPnLHandler _pnlHandler;
        private readonly ICacheService _cache; 

        public PortfolioController(GetPositionsHandler handler, GetPnLHandler pnlHandler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _pnlHandler = pnlHandler ?? throw new ArgumentNullException(nameof(pnlHandler));
        }

        [HttpGet("positions")]
        public async Task<ActionResult<IReadOnlyList<PositionResponse>>> GetPositions(CancellationToken ct)
        {
            const string cacheKey = "portfolio:position:v1";

            var cached = await _cache.GetAsync<List<PositionResponse>>(cacheKey, ct);

            if (cached is not null)
                return Ok(cached);

            var positions = await _handler.HandleAsync(ct);

            var response = positions
                .Select(p => new PositionResponse(p.Instrument, p.NetQuantity))
                .ToList();

            await _cache.SetAsync(cacheKey, response, TimeSpan.FromSeconds(60), ct);

            return Ok(response);
        }

        [HttpGet("pnl")]
        public async Task<ActionResult<PnLResponse>> GetPnL(CancellationToken ct)
        {
            /*Prepare Caching datas*/
            const string cacheKey = "portfolio:pnl:v1";
            var cached = await _cache.GetAsync<PnLResponse>(cacheKey, ct);
            if (cached is not null)
                return Ok(cached);

            //calling Handler to Get the data via the repository
            var summary = await _pnlHandler.HandleAsync(ct);

            //strucruring the data in a datastruct appropriate
            var response = new PnLResponse(
                summary.TotalNetCashFlow,
                summary.ByInstrument.Select(x => new PnLByInstrmentResponse(x.Instrument, x.NetCashFlow)).ToList());

            //Refresh/Update the cache with a new data
            await _cache.SetAsync(cacheKey, response, TimeSpan.FromSeconds(60), ct);

            return Ok(response);
        }

    }
}
