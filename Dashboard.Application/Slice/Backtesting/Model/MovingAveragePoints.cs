namespace Dashboard.Application.Slice.Backtesting.Model
{
    public record MovingAveragePoints
    {
        public decimal? maShortPeriod { get; set; }
        public decimal? maLongPeriod { get; set; }
        public decimal Close { get; set; }
        public string Date { get; set; }
    }
}
