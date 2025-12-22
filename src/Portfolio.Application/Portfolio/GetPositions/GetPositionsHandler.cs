using Portfolio.Application.Abstractions;
using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Portfolio.GetPositions
{
    public sealed class GetPositionsHandler
    {
        private readonly ITradeRepository _repo;

        public GetPositionsHandler(ITradeRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IReadOnlyList<PositionDto>> HandleAsync(CancellationToken ct)
        {
            var trades = await _repo.GetAllAsync(ct);

            var positions = trades
                .GroupBy(t => t.Instrument)
                .Select(g =>
                {
                    var netQty = g.Sum(t => t.Side == Side.Buy ? t.Quantity : -t.Quantity);
                    return new PositionDto(g.Key, netQty);
                })
                .OrderBy(p => p.Instrument)
                .ToList();

            return positions;
        }
    }
}
