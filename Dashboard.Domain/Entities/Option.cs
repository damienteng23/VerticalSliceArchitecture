namespace Dashboard.Domain.Entities
{
    public class Option
    {
        /// <summary>
        /// Current price of the asset (S)
        /// </summary>
        public decimal UnderlyingPrice { get; private set; }
        /// <summary>
        /// Exercise price (K)
        /// </summary>
        public decimal StrikePrice { get; private set; }
        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTime ExpiryDate { get; private set; }
        /// <summary>
        /// Annualized volatility (σ)
        /// </summary>
        public decimal Volatility { get; private set; }
        /// <summary>
        /// Annualized risk‑free rate (r)
        /// </summary>
        public decimal RiskFreeRate { get; private set; }
        /// <summary>
        /// Type of option
        /// </summary>
        public string OptionType { get; private set; }

        public Option(decimal underlyingPrice, decimal strikePrice, DateTime expiryDate, decimal volatility, decimal riskFreeRate, string optionType)
        {
            UnderlyingPrice = underlyingPrice;
            StrikePrice = strikePrice;
            ExpiryDate = expiryDate;
            Volatility = volatility;
            RiskFreeRate = riskFreeRate;
            OptionType = optionType;
        }
    }
}
