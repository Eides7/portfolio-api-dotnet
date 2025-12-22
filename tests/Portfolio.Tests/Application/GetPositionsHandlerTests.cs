using Moq;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Portfolio.GetPositions;
using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Tests.Application
{
    public class GetPositionsHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldAggregateNetQuantityByInstrument()
        {
            //Arrange
            var trades = new List<Trade>
            {
                new Trade(Guid.NewGuid(), "AAPL", 10, 100m, Side.Buy, new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                new Trade(Guid.NewGuid(), "AAPL", 3, 101m, Side.Sell, new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)),
                new Trade(Guid.NewGuid(), "MSFT", 5, 200m, Side.Buy, new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc))
            };

            var repo = new Mock<ITradeRepository>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(trades);

            var handler = new GetPositionsHandler(repo.Object);

            //Act
            var result = await handler.HandleAsync(CancellationToken.None);

            //Assert
            Assert.Equal(2, result.Count);

            var aapl = result.Single(x => x.Instrument == "AAPL");
            Assert.Equal(7, aapl.NetQuantity);

            var msft = result.Single(x => x.Instrument == "MSFT");
            Assert.Equal(5, msft.NetQuantity);
        }

        [Fact]
        public async Task HandleAsync_NoTrades_ShouldReturnEmptyList()
        {
            //Arrange
            var repo = new Mock<ITradeRepository>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Trade>());

            var handler = new GetPositionsHandler(repo.Object);

            //Act
            var result = await handler.HandleAsync(CancellationToken.None);

            //Assert
            Assert.Empty(result);
        }
    }
}
