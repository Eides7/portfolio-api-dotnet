using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Domain.Trading;
using Portfolio.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Repositories
{
    public sealed class EfTradeRepository : ITradeRepository
    {
        private readonly PortfolioDbContext _db;

        public EfTradeRepository(PortfolioDbContext db) 
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddAsync(Trade trade, CancellationToken ct)
        {
            await _db.Trades.AddAsync(trade, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<Trade>> GetAllAsync(CancellationToken ct)
        {
            return await _db.Trades
                .AsNoTracking()
                .OrderBy(t => t.TradeDateUtc)
                .ToListAsync(ct);
        }
    }
}
