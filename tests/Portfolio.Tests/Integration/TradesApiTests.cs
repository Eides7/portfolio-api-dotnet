using Portfolio.Api.Contracts;
using Portfolio.Domain.Trading;
using System.Net;
using System.Net.Http.Json;


namespace Portfolio.Tests.Integration
{
    public sealed class TradesApiTests : IClassFixture<CustomerWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TradesApiTests(CustomerWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Trades_SHouldReturn201Created()
        {
            //Arrange
            var request = new CreateTradeRequest(
                Instrument: "AAPL",
                Quantity: 10,
                Price: 100m,
                Side: Side.Buy,
                TradeDateUtc: new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
          );

            //Act
            var response = await _client.PostAsJsonAsync("/trades", request);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);


        }
    }
}
