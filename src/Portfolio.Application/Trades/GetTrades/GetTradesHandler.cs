using Portfolio.Application.Abstractions;
using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Trades.GetTrades
{
    public sealed class GetTradesHandler
    {
        private readonly ITradeRepository _repo;

        public GetTradesHandler(ITradeRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IReadOnlyList<Trade>> HandleAsync(CancellationToken ct)
        => _repo.GetAllAsync(ct);
    }
}
