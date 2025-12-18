using Portfolio.Domain.Trading;

namespace Portfolio.Api.Contracts
{
    public sealed record CreateTradeRequest(
        string Instrument,
        int Quantity,
        decimal Price,
        Side Side,
        DateTime TradeDateUtc);
}
