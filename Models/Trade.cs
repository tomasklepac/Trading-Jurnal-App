using SQLite;

namespace TradingJournalApp.Models
{
    /// <summary>
    /// Represents a trade in the trading journal.
    /// </summary>
    public class Trade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// The name of the instrument traded.
        /// </summary>
        public string Instrument { get; set; } = string.Empty;

        /// <summary>
        /// The date and time of the trade.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// The type of trade (e.g., Buy, Sell).
        /// </summary>
        public string Direction { get; set; } = string.Empty;

        /// <summary>
        /// The key level from which the trade was taken.
        /// </summary>
        public string KeyLevel { get; set; } = string.Empty;

        /// <summary>
        /// The type of confirmation for the trade.
        /// </summary>
        public string Confirmation { get; set; } = string.Empty;

        /// <summary>
        /// The type of continuation for the trade.
        /// </summary>
        public string Continuation { get; set; } = string.Empty;

        /// <summary>
        /// The result of the trade (e.g., Win, Loss).
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// The PnL (Profit and Loss) of the trade.
        /// </summary>
        public double PnL { get; set; } = 0.0;

        /// <summary>
        /// The link to a chart of the trade.
        /// </summary>
        public string Chart { get; set; } = string.Empty;

        /// <summary>
        /// The optional note for the trade.
        /// </summary>
        public string Note { get; set; } = string.Empty;
    }
}
