namespace Dashboard.Domain.Entities
{
    public class DailyTickerSummary
    {
         public string Symbol { get; set; }

        public DateTime From { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long Volume { get; set; }

        public decimal PreMarket { get; set; }

        public decimal AfterHours { get; set; }

        public string Status { get; set; }

        public DailyTickerSummary(string symbol, DateTime from, decimal open, decimal high, decimal low, decimal close, long volume, decimal preMarket, decimal afterHours, string status)
        {
            Symbol = symbol;
            From = from;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            PreMarket = preMarket;
            AfterHours = afterHours;
            Status = status;
        }
    }
}
