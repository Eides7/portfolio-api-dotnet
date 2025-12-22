namespace Portfolio.Api.Contracts
{
    public sealed record PositionResponse(string Instrument, int NetQuantity);
}
