namespace TradingJournalApp.Models
{
    /// <summary>
    /// Static class that contains options for trades.
    /// </summary>
    public static class TradeOptions
    {
        public static List<string> Directions => new() 
        { 
            "Long", "Short"
        };

        public static List<string> KeyLevels => new()
        {
            "London Low","London High", "Asia Low", "Asia High", "M15 FVG", "M15 BB", "M15 OB", "1H Low", "1H High", "1H FVG", "4H Low", "4H High", "4H FVG", "Daily Low", "Daily High"
        };

        public static List<string> Confirmations => new()
        {
            "BoS", "IFVG", "SMT"
        };

        public static List<string> Continuations => new()
        {
            "OB", "BB", "FVG", "EQ"
        };

        public static List<string> Results => new() 
        { 
            "WIN", "LOSS"
        };
    }
}
