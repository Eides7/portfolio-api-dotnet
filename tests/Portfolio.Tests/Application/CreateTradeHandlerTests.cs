using Moq;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Trades.CreateTrade;
using Portfolio.Domain.Trading;

namespace Portfolio.Tests.Application;

public class CreateTradeHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldCallRepository_AddAsync()
    {
        //Arrange
        var repo = new Mock<ITradeRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateTradeHandler(repo.Object);

        var cmd = new CreateTradeCommand(
            Instrument: "AAPL",
            Quantity: 10,
            Price: 100m,
            Side: Side.Buy,
            TradeDateUtc: new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            );

        //Act
        var result = await handler.HandleAsync(cmd, CancellationToken.None);

        //Assert
        Assert.NotEqual(Guid.Empty, result.TradeId);
        repo.Verify(r => r.AddAsync(It.Is<Trade>(t =>
        t.Instrument == "AAPL" &&
        t.Quantity == 10 &&
        t.Price == 100m &&
        t.Side == Side.Buy &&
        t.TradeDateUtc == cmd.TradeDateUtc
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}
