using Portfolio.Application.Abstractions;
using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Trades.CreateTrade
{
    public sealed class CreateTradeHandler
    {
        private readonly ITradeRepository _repo;

        public CreateTradeHandler(ITradeRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<CreateTradeResult> HandleAsync(CreateTradeCommand cmd, CancellationToken ct)
        {
            var tradeId = Guid.NewGuid();
            var trade = new Trade(
                tradeId,
                cmd.Instrument,
                cmd.Quantity,
                cmd.Price,
                cmd.Side,
                cmd.TradeDateUtc
                );

            await _repo.AddAsync(trade, ct);

            return new CreateTradeResult(tradeId);
        }
    }
}
