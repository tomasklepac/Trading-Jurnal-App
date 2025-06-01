namespace TradingJournalApp.Models
{
    /// <summary>
    /// Represents a trading day in the trading journal.
    /// </summary>
    public class TradeDay
    {
        /// <summary>
        /// The date of the trades.
        /// </summary>
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// The PnL (Profit and Loss) for the day.
        /// </summary>
        public double PnL { get; set; }

        /// <summary>
        /// The trading instruments of the day.
        /// </summary>
        public string Instruments { get; set; } = string.Empty;

        /// <summary>
        /// The number of trades opened on that day.
        /// </summary>
        public int TradesClosed { get; set; }

        /// <summary>
        /// Color for the final PnL value.
        /// </summary>
        public Color PnLColor => PnL >= 0 ? Colors.LimeGreen : Colors.Red;

        /// <summary>
        /// The date in raw format (DateTime).
        /// </summary>
        public DateTime DateRaw { get; set; }
    }
}

