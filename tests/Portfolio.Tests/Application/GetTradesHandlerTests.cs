using Moq;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Trades.GetTrades;
using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Tests.Application
{
    public class GetTradesHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnTradesFromRepository()
        {
            //Arrange
            var trades = new List<Trade>
            {
                new Trade(Guid.NewGuid(), "AAPL", 10, 100m, Side.Buy, new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                new Trade(Guid.NewGuid(), "MSFT", 5, 200m, Side.Sell, new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc))
            };

            var repo = new Mock<ITradeRepository>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(trades);

            var handler = new GetTradesHandler(repo.Object);

            //Act
            var result = await handler.HandleAsync(CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal("AAPL", result[0].Instrument);
            Assert.Equal("MSFT", result[1].Instrument);
        }
    }
}
