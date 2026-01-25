namespace Dashboard.Domain.Entities
{
    public class Greeks
    {
        /// <summary>
        /// Sensitivity to underlying price
        /// </summary>
        public decimal Delta { get; private set; }
        /// <summary>
        /// Sensitivity of Delta to underlying price
        /// </summary>
        public decimal Gamma { get; private set; }
        /// <summary>
        /// Sensitivity to volatility
        /// </summary>
        public decimal Vega { get; private set; }
        /// <summary>
        /// Sensitivity to time decay
        /// </summary>
        public decimal Theta { get; private set; }
        /// <summary>
        /// Sensitivity to interest rate
        /// </summary>
        public decimal Rho { get; private set; }

        public Greeks(decimal delta, decimal gamma, decimal vega, decimal theta, decimal rho)
        {
            Delta = delta;
            Gamma = gamma;
            Vega = vega;
            Theta = theta;
            Rho = rho;
        }
    }
}
