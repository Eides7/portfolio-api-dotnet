using Portfolio.Application.Abstractions;
using Portfolio.Domain.Trading;

namespace Portfolio.Infrastructure.Repositories
{
    public class InMemoryTradeRepository : ITradeRepository
    {
        private readonly List<Trade> _trades = new();

        public Task AddAsync(Trade trade, CancellationToken ct)
        {
            _trades.Add(trade);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Trade>> GetAllAsync(CancellationToken ct)
        {
            IReadOnlyList<Trade> snapshot = _trades.ToList();
            return Task.FromResult(snapshot);
        }
    }
}
