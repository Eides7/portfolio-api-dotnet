namespace Portfolio.Domain.Trading
{
    public sealed class Trade
    {
        public Guid Id { get; }
        public string Instrument { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public Side Side { get; }
        public DateTime TradeDateUtc { get; }

        
        public Trade(Guid id, string instrument, int quantity, decimal price,Side side, DateTime tradeDateUtc)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Trade id must not be empty.",nameof(id));
            if (string.IsNullOrWhiteSpace(instrument))
                throw new ArgumentException("Instrument is required.");
            if (quantity <= 0) throw new ArgumentException(nameof(quantity));
            if (price <= 0m) throw new ArgumentException(nameof(price));
            if (tradeDateUtc.Kind != DateTimeKind.Utc) throw new ArgumentException("TradeDateUtc must be UTC.", nameof(tradeDateUtc));

            Id = id;
            Instrument = instrument.Trim().ToUpperInvariant();
            Quantity = quantity;
            Price = price;
            Side = side;
            TradeDateUtc = tradeDateUtc;
        }
    }
}
