using Portfolio.Application.Abstractions;
using Portfolio.Domain.Trading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Portfolio.GetPnL
{
    public sealed class GetPnLHandler
    {
        private readonly ITradeRepository _repo;

        public GetPnLHandler(ITradeRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<PnLSummaryDto> HandleAsync(CancellationToken ct)
        {
            var trades = await _repo.GetAllAsync(ct);

            if (trades.Count == 0)
                return new PnLSummaryDto(0m, Array.Empty<PnLByInstrumentDto>());

            decimal CashFlow(Trade t)
                => (t.Side == Side.Sell ? 1m : -1m) * (t.Quantity * t.Price);

            var byInstrument = trades
                .GroupBy(t => t.Instrument)
                .Select(g => new PnLByInstrumentDto(
                    g.Key,
                    g.Sum(CashFlow)
                    ))
                .OrderByDescending(x => x.NetCashFlow)
                .ToList();

            var total = byInstrument.Sum(x => x.NetCashFlow);

            return new PnLSummaryDto(total, byInstrument);
               
        }
    }
}
