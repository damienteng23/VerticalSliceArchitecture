namespace Dashboard.Domain.Entities
{
    public class Tick
    {
        /// <summary>
        ///  Unique Identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Symbol identifier (e.g., "BTCUSDT")
        /// </summary>
        public string Symbol { get; set; } 
        /// <summary>
        /// The price at which the transaction occurred
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The amount of assets exchanged
        /// </summary>
        public decimal Size { get; set; }
        /// <summary>
        /// High-precision timestamp (UTC is standard in Finance)
        /// </summary>
        public DateTime Timestamp { get; set; }

        public Tick(string symbol, decimal price, decimal size, DateTime timestamp)
        {
            Symbol = symbol;
            Price = price;
            Size = size;
            Timestamp = timestamp;
        }
    }
}
