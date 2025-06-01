namespace TradingJournalApp.Models
{
    /// <summary>
    /// Daily PnL point for a graph visualization.
    /// </summary>
    public class DailyPnLPoint
    {
        /// <summary>
        /// Date of the trade.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Total PnL of the day.
        /// </summary>
        public double TotalPnL { get; set; }
    }
}
