namespace Dashboard.Application.Slice.Backtesting.Model
{
    public record MovingAveragePoints
    {
        public decimal? MaShortPeriod { get; set; }
        public decimal? MaLongPeriod { get; set; }
        public decimal Close { get; set; }
        public string Date { get; set; }
    }
}