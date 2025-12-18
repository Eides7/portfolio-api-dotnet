using Portfolio.Domain.Trading;

namespace Portfolio.Api.Contracts
{
    public sealed record TradeResponse(
        Guid Id,
        string Instrument,
        int Quantity,
        decimal Price,
        Side Side,
        DateTime TradeDateUtc
        );
}
