namespace Portfolio.Api.Contracts
{
    public sealed record PnLByInstrmentResponse(string Instrument, decimal NetCashFlow);

    public sealed record PnLResponse(decimal TotalNetCashFlow, IReadOnlyList<PnLByInstrmentResponse> ByInstrument);
}
