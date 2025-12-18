using Portfolio.Domain.Trading;

namespace Portfolio.Application.Abstractions
{
    public interface ITradeRepository
    {
        Task AddAsync(Trade trade, CancellationToken ct);
    }
}
