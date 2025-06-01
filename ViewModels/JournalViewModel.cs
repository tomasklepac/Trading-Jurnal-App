using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingJournalApp.Data;
using TradingJournalApp.Models;

namespace TradingJournalApp.ViewModels
{
    /// <summary>
    /// ViewModel for the Journal page.
    /// </summary>
    public class JournalViewModel : BaseViewModel
    {
        /// <summary>
        /// Repository for accessing trade data.
        /// </summary>
        private readonly TradeRepository repository;

        /// <summary>
        /// Collection of trade days.
        /// </summary>
        private ObservableCollection<TradeDay> tradeDays = new();
        public ObservableCollection<TradeDay> TradeDays
        {
            get => tradeDays;
            set
            {
                tradeDays = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command to navigate to the journal page.
        /// </summary>
        public ICommand GoToJournalCommand { get; }

        /// <summary>
        /// Command to navigate to the analytics page.
        /// </summary>
        public ICommand GoToAnalyticsCommand { get; }

        /// <summary>
        /// Constructor for the JournalViewModel.
        /// </summary>
        /// <param name="repo"></param>
        public JournalViewModel(TradeRepository repo)
        {
            repository = repo;
            TradeDays = new ObservableCollection<TradeDay>();

            GoToJournalCommand = new Command(() => {});

            GoToAnalyticsCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(Views.AnalyticsView));
            });

            _ = ReloadTradeDays();
        }

        /// <summary>
        /// Reloads the trade days from the repository.
        /// </summary>
        /// <returns></returns>
        public async Task ReloadTradeDays()
        {
            var trades = await repository.GetAllTradesAsync();

            var grouped = trades
                .GroupBy(t => t.Date.Date)
                .OrderByDescending(g => g.Key);

            var newTradeDays = new ObservableCollection<TradeDay>();

            foreach (var group in grouped)
            {
                var date = group.Key.ToString("dddd, MMMM dd");
                var pnl = group.Sum(t => t.PnL);
                var count = group.Count();

                var instruments = string.Join(", ", group
                    .Select(t => t.Instrument)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Distinct());

                newTradeDays.Add(new TradeDay
                {
                    Date = date,
                    DateRaw = group.Key,
                    PnL = pnl,
                    TradesClosed = count,
                    Instruments = instruments
                });
            }
            TradeDays = newTradeDays;
        }
    }
}
