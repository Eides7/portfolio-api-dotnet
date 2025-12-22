using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Persistence
{
    public sealed class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

        public DbSet<Trade> Trades => Set<Trade>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var trade = modelBuilder.Entity<Trade>();

            trade.ToTable("Trades");
            trade.HasKey(t => t.Id);

            trade.Property(t => t.Instrument)
                .IsRequired()
                .HasMaxLength(50);

            trade.Property(t => t.Quantity).IsRequired();
            trade.Property(t => t.Price).HasColumnType("decimal(18,4)");

            trade.Property(t => t.Side)
                .HasConversion<int>()
                .IsRequired();

            var utcConverter = new ValueConverter<DateTime, DateTime>(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            trade.Property(t => t.TradeDateUtc)
                .HasConversion(utcConverter)
                .IsRequired();
        }

    }
}
