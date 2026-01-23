using Moq;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Portfolio.GetPnL;
using Portfolio.Domain.Trading;
using Xunit;

namespace Portfolio.Tests.Application
{
    public class GetPnLHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnNetCashFlowByInstrument_AndTotal_SortedDescending()
        {
            var trades = new List<Trade>
            {   
                // AAPPL: BUY 10@100 => -1000; SELL 3@110 => +330 =>-670
                new Trade(Guid.NewGuid(), "AAPL", 10, 100m, Side.Buy, new DateTime(2025, 1,1,0,0,0,DateTimeKind.Utc)),
                new Trade(Guid.NewGuid(), "AAPL", 3, 110m, Side.Sell, new DateTime(2025, 1,1,0,0,0,DateTimeKind.Utc)),

                //MSFT: SELL 1@200 => +200
                new Trade(Guid.NewGuid(), "MSFT", 1, 200m, Side.Sell, new DateTime(2025, 1,3, 0, 0, 0, DateTimeKind.Utc))
            };

            var repo = new Mock<ITradeRepository>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(trades);

            var handler = new GetPnLHandler(repo.Object);

            //Act
            var result = await handler.HandleAsync(CancellationToken.None);

            //Assert total: -670 + 200 = -470;
            Assert.Equal(-470m, result.TotalNetCashFlow);

            //Assert per instrument
            Assert.Equal(2, result.ByInstrument.Count);

            //Sorted descending by NetCashFLow: MSFT(200) then AAPL(-670)
            Assert.Equal("MSFT", result.ByInstrument[0].Instrument);
            Assert.Equal(200m, result.ByInstrument[0].NetCashFlow);

            Assert.Equal("AAPL", result.ByInstrument[1].Instrument);
            Assert.Equal(-670m, result.ByInstrument[1].NetCashFlow);
        }

        [Fact]
        public async Task HandleAsync_NoTrades_ShouldReturnEmptyAndZeroTotal()
        {
            var repo = new Mock<ITradeRepository>();
            repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Trade>());

            var handler = new GetPnLHandler(repo.Object);

            var result = await handler.HandleAsync(CancellationToken.None);

            Assert.Equal(0m, result.TotalNetCashFlow);
            Assert.Empty(result.ByInstrument);
        } 
    }
}
