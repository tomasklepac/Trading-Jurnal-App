using SQLite;
using TradingJournalApp.Models;

namespace TradingJournalApp.Data
{
    /// <summary>
    /// Repository for managing trades in the SQLite database.
    /// </summary>
    public class TradeRepository
    {
        /// <summary>
        /// SQLite connection to the database.
        /// </summary>
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Constructor for the TradeRepository.
        /// </summary>
        /// <param name="dbPath"></param>
        public TradeRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Trade>().Wait();
        }

        /// <summary>
        /// Asynchronously retrieves all trades from the database.
        /// </summary>
        /// <returns></returns>
        public Task<List<Trade>> GetAllTradesAsync()
        {
            return _database.Table<Trade>()
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all trades from the database, ordered by date.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public Task<int> AddTradeAsync(Trade trade)
        {
            return _database.InsertAsync(trade);
        }

        /// <summary>
        /// Asynchronously deletes a trade from the database.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public Task<int> DeleteTradeAsync(Trade trade)
        {
            return _database.DeleteAsync(trade);
        }

        /// <summary>
        /// Asynchronously deletes a trade from the database.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public Task<int> UpdateTradeAsync(Trade trade)
        {
            return _database.UpdateAsync(trade);
        }

        /// <summary>
        /// Asynchronously retrieves a trade by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Trade?> GetTradeByIdAsync(int id)
        {
            var trades = await _database.Table<Trade>().ToListAsync();
            return trades.FirstOrDefault(t => t?.Id == id);
        }
    }
}
