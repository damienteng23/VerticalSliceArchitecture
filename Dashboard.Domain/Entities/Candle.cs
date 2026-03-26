namespace Dashboard.Domain.Entities
{
    public class Candle
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime Timestamp { get; internal set; }
        public decimal Open { get; internal set; }
        public decimal High { get; internal set; }
        public decimal Low { get; internal set; }
        public decimal Close { get; internal set; }
        public long Volume { get; internal set; }
        public decimal AdjustedClose { get; internal set; }

        public Candle(int id, string symbol, DateTime timestamp, decimal open, decimal high, decimal low, decimal close, long volume, decimal adjustedClose)
        {
            Id = id;
            Symbol = symbol;
            Timestamp = timestamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            AdjustedClose = adjustedClose;
        }
    }
}
